using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PDev.Auth.Api.Context;
using PDev.Auth.Api.Domain;
using PDev.Auth.Api.Models;
using PDev.Auth.Api.Service.Interface;

namespace PDev.Auth.Api.Service
{
    public class AuthorizationService : IAuthorizationService, IDisposable
    {
        private readonly SecurityContext _securityContext;

        public AuthorizationService()
        {
           // _securityContext = new SecurityContext();
        }

        public async Task<LoginResultModel> LogInAsync(string username, string password, string module = null)
        {
            return await Task.Run(() => LogIn(username, password, module));
        }

        public bool IsTokenValid(string userName, string token)
        {
            var tokenResult = _securityContext.UserTokens.FirstOrDefault(t => t.UserName == userName && t.AccessToken == token);
            if (tokenResult != null && !tokenResult.Revoke)
                return true;
            return false;
        }

        public User GetUser(string username)
        {
            return _securityContext.Users.Include(u => u.Roles).FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
        }
        public async Task<User> GetUserAsync(string username)
        {
            var user = await _securityContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
            return user;
        }
        public async Task LogUserAccess(UserToken token)
        {
            var existingToken = await _securityContext.UserTokens.FirstOrDefaultAsync(t => t.AccessToken == token.AccessToken);
            if (existingToken != null)
                return;
            _securityContext.UserTokens.Add(token);
            await _securityContext.SaveChangesAsync();
        }

        public bool IsValidUser(string username, string password)
        {
            var user = _securityContext.Users.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
            if (user != null && user.MatchPassword(password))
                return true;
            return false;
        }
        public bool IsModuleAuthorized(string userName, string moduleName)
        {
            var module = _securityContext.Modules.Include(m => m.Features).FirstOrDefault(m => m.Name == moduleName);

            if (module != null)
            {
                return _securityContext.Users
                    .Include(f => f.UserFeatures)
                    .Include("UserFeatures.Feature")
                    .FirstOrDefault(u => u.UserName == userName).UserFeatures.Any(f => f.Feature.IdModule == module.Id);
            }
            else
                return false;
        }
        public bool IsAuthorized(string userName, string controller, string action)
        {
            var user = _securityContext
                .Users
                .Include(f => f.Roles)
                .Include(f => f.UserFeatures)
                .FirstOrDefault(u => u.UserName == userName);

            if (user != null)
            {
                //Busca pelo acesso à funcionalidade especifica (UserFeature)
                var userFeature = user.UserFeatures.FirstOrDefault(f => f.Feature.Controller == controller);
                if (userFeature != null)
                {
                    //Verifica se acesso desbloqueado à funcionalidade
                    if (!userFeature.CanAccess)
                        return false;

                    //Verifica se a acao esta bloqueada
                    var featureAction = userFeature.ExcludedActions.Any(a => a.Action == action);
                    if (featureAction)
                        return false;

                    return true;
                }

                //Busca pela Role
                var roleFeature = user.Roles
                    .FirstOrDefault(r => r.Features.Any(f => f.Controller == controller));
                if (roleFeature != null)
                    return true;
            }
            return false;
        }

        public void Dispose()
        {
            if (_securityContext != null)
            {
                _securityContext.Dispose();
            }

        }
        public LoginResultModel LogIn(string username, string password, string module = null)
        {
            var user = GetUser(username);

            if (user == null)
            {
                return new LoginResultModel(false, "Usuário inexistente");
            }
            if (user.Status == UserStatus.PendingActivation)
            {
                return new LoginResultModel(false, "Seu usuário não foi ativado. Caso não tenha recebido o e-mail de ativação, clique em 'Quero me cadastrar' -> 'Reenviar chave de ativação'");
            }
            if (user.Status == UserStatus.Inactive)
            {
                return new LoginResultModel(false, "Usuário bloqueado. Por favor, entre em contato com seu analista.");
            }
            if (!user.MatchPassword(password))
            {
                return new LoginResultModel(false, "Usuário ou senha inválidos");
            }
            if (user.Level != UserLevel.Admin && user.Level != UserLevel.SysAdmin)
            {
                if (!string.IsNullOrEmpty(module))
                {
                    if (!IsModuleAuthorized(username, module))
                    {
                        return new LoginResultModel(false, "Seu usuário não possui permissão de acesso a este módulo");
                    }
                }
            }


            var result = new LoginResultModel(true, "Ok")
            {
                User = user
            };

            return result;
        }
    }
}