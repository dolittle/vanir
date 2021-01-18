using System.Globalization;
using System.Threading;
using ExecutionContext = Dolittle.SDK.Execution.ExecutionContext;

namespace Dolittle.Vanir.Backend.Execution
{
    public static class ExecutionContextManager
    {
        private static AsyncLocal<ExecutionContext> _currentExecutionContext = new AsyncLocal<ExecutionContext>();
        public static ExecutionContext CurrentExecutionContext => _currentExecutionContext.Value;

        public static ExecutionContext Establish()
        {
            _currentExecutionContext.Value = new ExecutionContext(
                "824d28bc-700a-4d1d-ada9-7e6d9102519f",
                "53a2a95e-d5fd-4aea-932a-1962a6a9d096",
                new Dolittle.SDK.Microservices.Version(1, 0, 0, 0, ""),
                Dolittle.SDK.Microservices.Environment.Development,
                "470e414e-2a2b-4692-9770-e65bd9757874",
                Dolittle.SDK.Security.Claims.Empty,
                CultureInfo.InvariantCulture);

            return _currentExecutionContext.Value;
        }

        public static void Set(ExecutionContext context) => _currentExecutionContext.Value = context;
    }
}
