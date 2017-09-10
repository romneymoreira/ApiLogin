using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using PDev.Auth.Api.Service.Interface;

namespace PDev.Auth.Api.Service
{
    public sealed class ApiDeUsuarios : IApiDeUsuarios
    {
        public void Invocar(string acao, object dados)
        {
            var json = new JavaScriptSerializer().Serialize(dados);

            using (var client = new HttpClient())
            using (var postData = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                var userApi = System.Configuration.ConfigurationManager.AppSettings["AdminApiUsers"];
                var url = userApi + (userApi.Last() == '/' ? acao : "/" + acao);

                var result = client.PostAsync(url, postData).Result;

                if (result.StatusCode != System.Net.HttpStatusCode.OK
                    && result.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    throw new System.Exception(result.ReasonPhrase);
                }
            }
        }
    }  
}