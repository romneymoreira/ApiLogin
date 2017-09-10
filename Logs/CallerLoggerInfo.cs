using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDev.Auth.Api.Logs
{
    public sealed class CallerLoggerInfo
    {
        public CallerLoggerInfo(string origem, string actionUrl, string mensagem)
        {
            this.Origem = origem;
            this.Action = actionUrl;
            this.Mensagem = mensagem;
        }

        public string Action { get; private set; }
        public string Mensagem { get; private set; }
        public string Origem { get; private set; }
    }
}