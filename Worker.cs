using Microsoft.Extensions.Hosting;

namespace CarStore
{
    public class Worker : IHostedService
    {
        private readonly IDoStuff _doStuff;

        public Worker(IDoStuff doStuff)
        {
            _doStuff = doStuff;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _doStuff.SelectOutput();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _doStuff.DisplayOptions();
            return Task.CompletedTask;
        }
    }
}
