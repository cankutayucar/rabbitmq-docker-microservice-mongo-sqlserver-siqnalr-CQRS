using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.PipelimeBehaviours
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        public PerformanceBehavior()
        {
            _timer = Stopwatch.StartNew();
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();
            var respponse = await next();
            _timer.Stop();
            var elapseMilliseconds = _timer.ElapsedMilliseconds;
            if(elapseMilliseconds > 1000)
            {
                var requestName = typeof(TRequest).Name;
                // logger
            }
            return respponse;
        }
    }
}
