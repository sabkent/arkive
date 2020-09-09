using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace pws.Shared.Hubs
{
    public sealed class RetryForeverPolicy : IRetryPolicy
    {
        public static RetryForeverPolicy Create() => new RetryForeverPolicy();


        public TimeSpan? NextRetryDelay(RetryContext retryContext)=> 
            TimeSpan.FromMinutes(1);
        
    }
}
