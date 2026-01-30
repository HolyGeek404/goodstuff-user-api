using GoodStuff.UserApi.Application.Services.Interfaces;

namespace GoodStuff.UserApi.Application.Services;

public class GuidProvider : IGuidProvider
{
    public Guid Get()
    {
        var guid = Guid.NewGuid();
        return guid;
    }
    
}