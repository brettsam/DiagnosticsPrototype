using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace ExtensionPrototype
{
    internal class AzureFunctionsDiagnosticsEventHandler : IObserver<DiagnosticListener>, IObserver<KeyValuePair<string, object?>>, IDisposable
    {
        private readonly IList<IDisposable> _subscription = new List<IDisposable>();

        public AzureFunctionsDiagnosticsEventHandler()
        {
            _subscription.Add(DiagnosticListener.AllListeners.Subscribe(this));
        }

        public void OnNext(DiagnosticListener listener)
        {
            if (listener.Name == "Microsoft.Azure.Functions")
            {
                listener.Subscribe(this);
            }
        }

        public void OnNext(KeyValuePair<string, object> evt)
        {
            switch (evt.Key)
            {
                case "Microsoft.Azure.Functions.ColdStartRequest.Start":
                    StartColdStartRequest(evt.Value as HttpContext);
                    break;
                case "Microsoft.Azure.Functions.ColdStartRequest.Stop":
                    StopColdStartRequest(evt.Value as HttpContext);
                    break;
                default:
                    break;
            }
        }

        internal void StartColdStartRequest(HttpContext context)
        {
            if (context != null)
            {
                Console.WriteLine($"{nameof(StartColdStartRequest)} -- {context.Request.Path.Value}");
            }
        }

        internal void StopColdStartRequest(HttpContext context)
        {
            if (context != null)
            {
                Console.WriteLine($"{nameof(StopColdStartRequest)} -- {context.Request.Path.Value}");
            }
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void Dispose()
        {
            foreach (var sub in _subscription)
            {
                sub.Dispose();
            }
        }
    }
}
