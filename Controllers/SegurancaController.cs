using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using PDev.Auth.Api.Models;
using PDev.Auth.Api.Service;
using System.Security.Claims;
using PDev.Auth.Api.Service.Interface;

namespace PDev.Auth.Api.Controllers
{
    [Authorize]
    [RoutePrefix("seguranca")]
    public class SegurancaController : ApiController
    {
        private readonly IUserService userService;

        public SegurancaController()
        {
            userService = new UserService();
        }


        [HttpPost]
        [Route("alterarsenha")]
        public string ChangePassword(AlterarSenhaModel senhas)
        {
            var claims = (RequestContext.Principal as ClaimsPrincipal).Claims;
            var id = int.Parse(claims.FirstOrDefault(c => c.Type == "id_user").Value);

            return userService.ChangePassword(id, senhas.SenhaAtual, senhas.NovaSenha);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("enivarnovasenha")]
        public string SendNewPassword(SendNewPasswordModel model)
        {
            IApiDeUsuarios seguranca = new ApiDeUsuarios();
            seguranca.Invocar("enivarnovasenha", model);

            return "Senha enviada para o email informado.";
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ativarnovasenha")]
        public string ActivateUserByToken(ActivateUserModel model)
        {
            IApiDeUsuarios seguranca = new ApiDeUsuarios();
            seguranca.Invocar("ativarnovasenha", model);

            return "Sua senha foi alterada com sucesso.";
        }        
    }
}