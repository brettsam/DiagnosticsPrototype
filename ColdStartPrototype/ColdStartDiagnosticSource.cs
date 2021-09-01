using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace ColdStartPrototype
{
    public class ColdStartDiagnosticSource
    {
        public const string DiagnosticListenerName = "Microsoft.Azure.Functions";
        public const string ColdStartEventName = "Microsoft.Azure.Functions.ColdStartRequest";

        private static readonly DiagnosticListener _diagnosticListener = new DiagnosticListener(DiagnosticListenerName);

        internal Activity RequestStart(HttpContext context)
        {
            // Always raise this, even if no one is listening.
            // This will not be called after we've specialized.
            Activity activity = new Activity(ColdStartEventName);
            _diagnosticListener.StartActivity(activity, context);

            return activity;
        }

        internal void RequestStop(Activity activity, HttpContext context)
        {
            if (activity != null)
            {
                _diagnosticListener.StopActivity(activity, context);
            }
        }

    }
}
