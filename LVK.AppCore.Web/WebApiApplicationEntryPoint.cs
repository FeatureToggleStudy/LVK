using System;
using System.Threading;
using System.Threading.Tasks;

namespace LVK.AppCore.Web
{
    public class WebApiApplicationEntryPoint : IApplicationEntryPoint
    {
        public Task<int> Execute(CancellationToken cancellationToken)
        {
            throw new Exception();
        }
    }
}