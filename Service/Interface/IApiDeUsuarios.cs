using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDev.Auth.Api.Service.Interface
{
    public interface IApiDeUsuarios
    {
        void Invocar(string acao, object dados);
    }
}
