using System.Security.Cryptography;
using GoodStuff.UserApi.Application.Models;
using GoodStuff.UserApi.Application.Services.Interfaces;
using GoodStuff.UserApi.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace GoodStuff.UserApi.Application.Services;

public class UserSessionService(
    IMemoryCache cache,
    IHttpContextAccessor? httpContextAccessor,
    ILogger<UserSessionService> logger) : IUserSessionService
{
    private const int SessionTimeoutMinutes = 30;

    public UserSession CreateSession(User user)
    {
        try
        {
            var sessionId = GenerateSecureSessionId();
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(SessionTimeoutMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(15),
                Priority = CacheItemPriority.High
            };
            var userSession = new UserSession
            {
                Email = user.Email.Value,
                UserId =  user.Id,
                SessionId = sessionId,
                LastActivity = DateTime.UtcNow,
                LoginTime = DateTime.UtcNow,
                IpAddress = GetClientIpAddress(),
            };

            cache.Set(GetCacheKey(sessionId), userSession, cacheOptions);
            return userSession;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating user session for {UserEmail}", user.Email);
            throw;
        }
    }

    public UserSession? GetUserSession(string? session = null)
    {
        try
        {
            var sessionId = session == null ? GetSessionIdFromCookie() : null;
            if (sessionId == null) return null;

            var cachedKey = GetCacheKey(sessionId);
            cache.TryGetValue(cachedKey, out UserSession? userSession);

            return userSession;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during getting session");
            throw;
        }
    }

    public bool Validate()
    {
        try
        {
            var session = GetUserSession();
            if (session == null) return false;

            var sessionAge = DateTime.UtcNow - session.LastActivity;
            var sessionId = GetSessionIdFromCookie();
            
            if (sessionAge.TotalMinutes > SessionTimeoutMinutes)
            {
                if (sessionId != null) ClearUserCachedData(sessionId);
                return false;
            }
            session.LastActivity = DateTime.UtcNow;

            var currentIp = GetClientIpAddress();
            if (currentIp == session.IpAddress) 
                return true;
        
            sessionId = GetSessionIdFromCookie();
            if (sessionId != null) ClearUserCachedData(sessionId);
            return false;
            
        }
        catch (Exception ex)
        {
            logger.LogError("Couldn't validate session because: {Exception}", ex);
            throw;
        }
    }

    public void ClearUserCachedData(string sessionId)
    {
        if (!string.IsNullOrEmpty(sessionId))
            cache.Remove(GetCacheKey(sessionId));
        logger.LogInformation("User session cleared");
    }

    #region Private

    private string? GetSessionIdFromCookie()
    {
        return httpContextAccessor?.HttpContext?.Request.Cookies["UserSessionId"];
    }

    private static string GetCacheKey(string sessionId)
    {
        return $"user_session_{sessionId}";
    }

    private static string GenerateSecureSessionId()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }

    private string GetClientIpAddress()
    {
        if (httpContextAccessor == null) return "unknown";

        var forwardedFor = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor)) return forwardedFor.Split(',')[0].Trim();

        return httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    #endregion
}