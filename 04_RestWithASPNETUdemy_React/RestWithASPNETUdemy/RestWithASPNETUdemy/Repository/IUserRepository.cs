using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;

namespace RestWithASPNETUdemy.Repository
{
    public interface IUserRepository
    {
        User ValidateUserCredentials(UserVO user);
        User ValidateUserNameCredentials(string userName);
        bool RevokeToken(string userName);
        User UpdateUser(User user);
    }
}
