using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDev.Auth.Api.Domain
{
    /// <summary>
    /// Classe para controle de AccessTokens (OAuth)
    /// 
    /// A propriedade 'Revoke' deve ser utilizada quando se quer revogar o acesso de um token de usuário,
    /// como por exemplo, quando este altera a senha ou tem suas permissões alteradas.
    /// Alternativa simples ao uso de RefreshTokens
    /// </summary>
    public class UserToken
    {
        public int Id { get; private set; }
        public string AccessToken { get; private set; }
        public string UserName { get; private set; }
        public DateTime Expiration { get; private set; }
        public bool Revoke { get; private set; }

        protected UserToken() { }
        public UserToken(string accessToken, string userName, DateTime expiration)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException("accessToken");
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            if (expiration == DateTime.MinValue)
                throw new ArgumentNullException("expiration");

            this.AccessToken = accessToken;
            this.UserName = userName;
            this.Expiration = expiration;
        }

        public void RevokeAccess()
        {
            this.Revoke = true;
        }
    }
}