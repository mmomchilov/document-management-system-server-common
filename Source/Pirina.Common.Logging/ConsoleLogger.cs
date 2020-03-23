using System;
using System.Threading.Tasks;
using Glasswall.Kernel.Logging;

namespace Glasswall.Common.Logging
{
    [Obsolete("Use add console instead.", true)]
    public class ConsoleLogger : IEventLogger
    {
        private const string NullExceptionText = "'Unknown Exception'";
        private const string NullEventSourceText = "'Unknown EventSource'";
        private const string Delimiter = "|";

        public Task Log(SeverityLevel level, Enum eventId, Type eventSource, Guid transactionId, string message)
        {
            var source = eventSource == null ? NullEventSourceText : eventSource.ToString();

            SetConsoleColours(level);

            Console.WriteLine($"{DateTime.UtcNow} {Delimiter} {level} {Delimiter} {eventId} {Delimiter} {source} {Delimiter} {transactionId} {Delimiter} {message}");
            return Task.CompletedTask;
        }

        public Task Log(SeverityLevel level, Enum eventId, Type eventSource, string message)
        {
            Log(level, eventId, eventSource, Guid.Empty, message);
            return Task.CompletedTask;
        }

        public Task Log(SeverityLevel level, Enum eventId, Type eventSource, Guid transactionId, Exception exception)
        {
            var exceptionText = exception == null ? NullExceptionText : exception.ToString();

            Log(level, eventId, eventSource, transactionId, exceptionText);
            return Task.CompletedTask;
        }

        public Task Log(SeverityLevel level, Enum eventId, Type eventSource, Exception exception)
        {
            Log(level, eventId, eventSource, Guid.Empty, exception);
            return Task.CompletedTask;
        }

        private static void SetConsoleColours(SeverityLevel level)
        {
            Console.ResetColor();

            switch (level)
            {
                case SeverityLevel.Critical:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case SeverityLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case SeverityLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case SeverityLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case SeverityLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case SeverityLevel.Trace:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
        }
    }
}