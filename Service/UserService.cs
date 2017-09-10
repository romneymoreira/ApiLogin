using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PDev.Auth.Api.Domain;
using PDev.Auth.Api.Service.Interface;

namespace PDev.Auth.Api.Service
{
    public class UserService : IUserService
    {
        public UserService()
        {
        }

        public User ActivateUserByToken(int userId, string token, string newPassword)
        {
            throw new NotImplementedException();
        }

        public string ChangePassword(int userId, string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public Role GetRoleByIdentifier(string identifier)
        {
            throw new NotImplementedException();
        }

        public User GetUser(int userId)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string username)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public void SaveUser(User user)
        {
            throw new NotImplementedException();
        }

        public void SendNewPassword(string email)
        {
            throw new NotImplementedException();
        }
    }
}