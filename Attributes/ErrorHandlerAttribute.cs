using log4net;
using PDev.Auth.Api.Logs;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace PDev.Auth.Api.Attributes
{
    public sealed class ErrorHandlerAttribute : ExceptionFilterAttribute
    {
        private readonly ILog log;

        public ErrorHandlerAttribute(ILog log)
        {
            this.log = log;
        }

       // public async override Task OnExceptionAsync(HttpActionExecutedContext context, CancellationToken cancellationToken)
      //  {
        //    await Task.Run(() =>
        //    {
        //        CallerLoggerInfo mensagem;
        //        context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, context.Exception.Message);
        //        var origem = context.Request.Headers.Referrer != null ? context.Request.Headers.Referrer.AbsoluteUri : "Origem não identificada";

        //        if (context.Exception.GetType() == typeof(NullReferenceException))
        //        {
        //            mensagem = new CallerLoggerInfo(origem, context.Request.RequestUri.ToString(), "Ocorreu um erro não esperado durante o processamento da sua requisição. Contate o time de suporte explicando o que você estava fazendo.");
        //            context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ocorreu um erro durante o processamento da sua requisição. Por favor, tente novamente.");
        //        }
        //        if (typeof(System.Data.DataException).IsAssignableFrom(context.Exception.GetType()))
        //        {
        //            mensagem = new CallerLoggerInfo(origem, context.Request.RequestUri.ToString(), "Ocorreu um erro durante o processamento da sua requisição. Por favor, tente novamente.");
        //            context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Conflict, "Ocorreu um erro durante o processamento da sua requisição. Por favor, tente novamente.");
        //        }
        //        else
        //        {
        //            mensagem = new CallerLoggerInfo(origem, context.Request.RequestUri.ToString(), context.Exception.Message);
        //        }

        //        log.Error(mensagem, context.Exception);
        //    });
        //}        
   }

}