using System;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Api.Services.Utils
{
    public static class BackgroundJobService
    {
        public static async void RepeatActionEvery(Action action, TimeSpan interval, CancellationToken cancellationToken)
        {
            while (true)
            {
                action();
                Task task = Task.Delay(interval, cancellationToken);

                try
                {
                    await task;
                }

                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }
    }
}
