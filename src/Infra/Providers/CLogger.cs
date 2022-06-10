using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using nCroner.Common;

namespace Infra.Providers
{
    public class CLogger : ICLogger
    {
        private const string Msg = "{@Message} {@File} {@Method} {@Line} {@Request} {@Response}";

        private readonly ILogger<CLogger> _logger;

        public CLogger(ILogger<CLogger> logger)
        {
            _logger = logger;
        }

        public void Trace(string message, object? request = null, object? response = null,
            [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "",
            [CallerFilePath] string sourceFilePath = "")
        {
            WriteLog(LogLevel.Trace, message, request, response, lineNumber, methodName, sourceFilePath);
        }

        public void Debug(string message, object? request = null, object? response = null,
            [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "",
            [CallerFilePath] string sourceFilePath = "")
        {
            WriteLog(LogLevel.Debug, message, request, response, lineNumber, methodName, sourceFilePath);
        }

        public void Info(string message, object? request = null, object? response = null,
            [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "",
            [CallerFilePath] string sourceFilePath = "")
        {
            WriteLog(LogLevel.Information, message, request, response, lineNumber, methodName, sourceFilePath);
        }

        public void Error(string message, object? request = null, object? response = null,
            [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "",
            [CallerFilePath] string sourceFilePath = "")
        {
            WriteLog(LogLevel.Error, message, request, response, lineNumber, methodName, sourceFilePath);
        }

        public void Error(string message, Exception exception, object? request = null,
            [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "",
            [CallerFilePath] string sourceFilePath = "")
        {
            WriteLog(LogLevel.Error, message, request, exception, lineNumber, methodName, sourceFilePath);
        }

        private void WriteLog(LogLevel level, string message, object? request, object? response = null,
            int? lineNumber = 0, string methodName = "", string sourceFilePath = "")
        {
            try
            {
                _logger.Log(
                    level,
                    Msg,
                    sourceFilePath,
                    methodName,
                    lineNumber,
                    message,
                    request,
                    response);
            }
            catch
            {
                //
            }
        }

    }
}