using Identity.Api.DataAccess.Models;

namespace Identity.Api.Services
{
    public interface IMembershipService
    {
        User Create(string userName, string password);
        User GetUserByName(string userName);
        User GetUserById(long userId);
        User LoginUser(string userName, string password);
    }
}
