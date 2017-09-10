using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace PDev.Auth.Api.Domain
{
    public enum UserLevel
    {
        SysAdmin = 5,       //Administrador Técnico (FullAdmin)
        Admin = 4,          //Administrador do sistema
        Manager = 3,        //Acessa todas as funcionalidades do sistema, exceto funcionalidades de administração - Nao requer role associada
        SuperUser = 2,      //Acessa todas as funcionalidades do sistema, mas só altera as do seu papel (role) e definidas no seu usuario (UserFeature)
        User = 1            //Usuário padrão - Acessa apenas as funcionalidades definidas no seu papel(role) e funcionalidades definidas no seu usuario (UserFeature)
    }

    public enum UserStatus
    {
        Active = 1,              //Ativo
        Inactive = 0,            //Inativo
        PendingActivation = 2    //Pendente de ativação
    }

    public class User
    {
        public int Id { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Name { get; private set; }
        public string Mobile { get; private set; }
        public string Email { get; private set; }
        public string Token { get; private set; }
        public DateTime? TokenExpiration { get; private set; }
        public UserLevel Level { get; private set; }
        public UserStatus Status { get; private set; }

        //Perfis do Usuário
        public ICollection<Role> Roles { get; private set; }
        public ICollection<UserFeature> UserFeatures { get; private set; }

        protected User() { }

        public User(string userName, string password, string name, string email)
        {
            if (userName == null)
                throw new ArgumentNullException("userName");
            if (password == null)
                throw new ArgumentNullException("password");
            if (name == null)
                throw new ArgumentNullException("name");
            if (email == null)
                throw new ArgumentNullException("email");

            if (userName.Length < 4)
                throw new System.Exception("O nome de usuário deve conter ao menos 4 caracteres");
            if (userName.Length > 25)
                throw new System.Exception("O nome de usuário deve conter no máximo 25 caracteres");
            if (!ValidEmail(email))
                throw new System.Exception("O e-mail especificado não é válido");

            this.UserName = userName;
            this.Name = name;
            this.Email = email;
            this.Status = UserStatus.Active;
            SetPassword(password);
            RequestValidationToken();
        }

        public User(string userName, string password, string name, string email, string mobile)
            : this(userName, password, name, email)
        {
            if (mobile == null)
                throw new ArgumentNullException("mobile");

            this.Mobile = CheckAndTreatMobile(mobile);
        }

        /// <summary>
        /// Adiciona uma Feature específica a um usuario
        /// </summary>
        /// <param name="feature">Feature</param>
        /// <param name="canAccess">Booleano se pode acessar ou não a funcionalidade</param>
        public void AddFeature(Feature feature, bool canAccess = true)
        {
            if (feature == null)
                throw new ArgumentNullException("feature");

            if (UserFeatures == null)
                UserFeatures = new List<UserFeature>();

            if (UserFeatures.All(f => f.FeatureId != feature.Id))
            {
                var userFeature = new UserFeature(feature, canAccess);
                UserFeatures.Add(userFeature);
            }
        }

        internal bool IsActive()
        {
            return Status == UserStatus.Active;
        }

        /// <summary>
        /// Remove uma Featura específica de um usuário
        /// </summary>
        /// <param name="featureId">Id da Feature</param>
        public void RemoveFeature(int featureId)
        {
            if (UserFeatures == null)
                return;

            var feature = UserFeatures.FirstOrDefault(f => f.FeatureId == featureId);
            if (feature != null)
                UserFeatures.Remove(feature);
        }

        /// <summary>
        /// Adiciona um Perfil à um usuário
        /// </summary>
        /// <param name="role">Perfil</param>
        public void AddRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            if (Roles == null)
                Roles = new List<Role>();

            if (Roles.All(r => r.Id != role.Id))
                this.Roles.Add(role);
        }

        /// <summary>
        /// Remove um perfil de um usuário
        /// </summary>
        /// <param name="roleId">Id do Perfil</param>
        public void RemoveRole(int roleId)
        {
            if (this.Roles == null)
                return;

            var role = this.Roles.FirstOrDefault(r => r.Id == roleId);
            if (role != null)
                this.Roles.Remove(role);
        }

        /// <summary>
        /// Ativa um usuário
        /// </summary>
        public void Activate()
        {
            this.Token = null;
            this.TokenExpiration = null;
            Status = UserStatus.Active;
        }

        /// <summary>
        /// Ativa um usuário alterando sua senha
        /// </summary>
        /// <param name="password">Nova senha</param>
        public void Activate(string password)
        {
            Activate();
            SetPassword(password);
        }

        /// <summary>
        /// Desativa um usuário
        /// </summary>
        public void Deactivate()
        {
            Status = UserStatus.Inactive;
        }

        /// <summary>
        /// Cria um novo token de validação para o usuário
        /// </summary>
        /// <param name="isPasswordReset">Booleano indicando se é reset de senha ou não. Caso positivo, o status do usuário permanece inalterado.</param>
        public void RequestValidationToken(bool isPasswordReset = false)
        {
            this.Token = Guid.NewGuid().ToString("N");
            this.TokenExpiration = DateTime.Now.AddMonths(1);
            if (!isPasswordReset)
                this.Status = UserStatus.PendingActivation;
        }

        /// <summary>
        /// Indica se existe token associado
        /// </summary>
        /// <returns>True para token associado</returns>
        public bool HasRequestedToken()
        {
            return !string.IsNullOrEmpty(this.Token);
        }

        /// <summary>
        /// Valida um token
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>True para token válido</returns>
        public bool MatchToken(string token)
        {
            return this.Token == token;
        }

        /// <summary>
        /// Verifica se um token está expirado
        /// </summary>
        /// <returns>True para token na validade</returns>
        public bool IsTokenUpDate()
        {
            return this.TokenExpiration >= DateTime.Now;
        }

        /// <summary>
        /// Altera a senha do usuário
        /// </summary>
        /// <param name="oldPassword">Senha antiga</param>
        /// <param name="newPassword">Nova senha</param>
        /// <returns>Mensagem vazia em caso de alteração de sucesso e exceção em caso de erro</returns>
        public string ChangePassword(string oldPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(oldPassword)) { throw new ArgumentException("A senha atual deve ser diferente de nulo e vazio."); }
            if (string.IsNullOrWhiteSpace(newPassword)) { throw new ArgumentException("A nova senha deve ser diferente de nulo e vazio."); }
            var hashadOldPassword = Hash(oldPassword);
            if (hashadOldPassword != Password)
            {
                throw new System.Exception();
            }
            if (hashadOldPassword == Hash(newPassword))
            {
                throw new System.Exception("A senha atual e a nova senha são iguais.");
            }


            SetPassword(newPassword);
            return "";
        }

        public void ChangeMobileNumber(string password, string newMobileNumber)
        {
            if (Hash(password) != this.Password)
                throw new System.Exception();

            this.Mobile = CheckAndTreatMobile(newMobileNumber);
        }

        public bool MatchPassword(string password)
        {
            return Hash(password) == this.Password;
        }

        public static string Hash(string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value)));
        }

        public static string CreateUrl(string baseUrl, params string[] urlParams)
        {
            return baseUrl + urlParams.Aggregate(string.Empty, (current, s) => current + ("/" + s));
        }

        #region Private Methods
        private void SetPassword(string password)
        {
            if (password.Length < 6 || password.Length > 20)
                throw new System.Exception("A senha deve conter entre 6 e 20 caracteres.");
            this.Password = Hash(password);
        }

        private string CheckAndTreatMobile(string mobile)
        {
            //Recupera somente os digitos
            mobile = Regex.Replace(mobile, @"[^\d]", "");

            //Se ddd começa com zero, remove
            if (mobile.StartsWith("0"))
                mobile = mobile.Remove(0, 1);

            //Valida se é telefone valido
            if (mobile.Length < 10)
                throw new System.Exception("O telefone digitado é inválido");

            //Valida se o numero é celular
            int phoneRange = int.Parse(mobile.Substring(2, 1));
            if (phoneRange < 7)
                throw new System.Exception("O telefone digitado deve ser um celular");

            return mobile;
        }

        private bool ValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
              + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
              + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        #endregion
    }
}