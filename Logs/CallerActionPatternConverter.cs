using log4net.Core;
using log4net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PDev.Auth.Api.Logs
{
    public sealed class CallerActionPatternConverter : PatternConverter
    {
        protected override void Convert(TextWriter writer, object state)
        {
            if (state == null)
            {
                writer.Write(SystemInfo.NullText);
                return;
            }

            var loggingEvent = state as LoggingEvent;
            var actionInfo = loggingEvent == null ? null : loggingEvent.MessageObject as CallerLoggerInfo;

            if (actionInfo == null)
            {
                writer.Write(loggingEvent.MessageObject);
            }
            else
            {
                writer.Write(string.Format("[{0}] [{1}] [{2}]", actionInfo.Origem, actionInfo.Action, actionInfo.Mensagem));
            }
        }
    }
}