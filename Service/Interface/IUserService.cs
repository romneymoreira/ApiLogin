using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDev.Auth.Api.Domain;

namespace PDev.Auth.Api.Service.Interface
{
    public interface IUserService
    {
        User ActivateUserByToken(int userId, string token, string newPassword);
        string ChangePassword(int userId, string currentPassword, string newPassword);
        void DeleteUser(int userId);
        Role GetRoleByIdentifier(string identifier);
        User GetUser(int userId);
        User GetUser(string username);
        User GetUserByEmail(string email);
        IEnumerable<User> GetUsers();
        void SaveUser(User user);
        void SendNewPassword(string email);
    }
}
