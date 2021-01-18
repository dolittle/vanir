using Microsoft.AspNetCore.Builder;

namespace Dolittle.Vanir.Backend.Execution
{
    public static class ExecutionContextExtensions
    {
        public static void UseExecutionContext(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                ExecutionContextManager.Establish();
                await next.Invoke().ConfigureAwait(false);
            });
        }
    }
}
