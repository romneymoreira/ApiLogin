using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDev.Auth.Api.Domain;

namespace PDev.Auth.Api.Service.Interface
{
    public interface IAuthorizationService
    {
        User GetUser(string username);
        bool IsAuthorized(string userName, string controller, string action);
    }
}
