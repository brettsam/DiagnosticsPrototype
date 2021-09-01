using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ColdStartPrototype
{
    public class ColdStartDiagnosticsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ColdStartDiagnosticSource _diagnosticSource = new ColdStartDiagnosticSource();

        public ColdStartDiagnosticsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Activity act = Activity.Current;

            Activity activity = _diagnosticSource.RequestStart(context);

            try
            {
                await _next(context);
            }
            finally
            {
                _diagnosticSource.RequestStop(activity, context);
            }
        }
    }
}
