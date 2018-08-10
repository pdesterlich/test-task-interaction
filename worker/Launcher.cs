using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace worker
{
    public class Launcher
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private List<Task> _tasks;
        private readonly List<BufferBlock<string>> _queues;

        public Launcher()
        {
            _queues = new List<BufferBlock<string>>();

            _cancellationTokenSource = new CancellationTokenSource();
            _tasks = new List<Task>();

            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/hub")
                .Build();

            connection.On<string>("message", HandleMessage);

            connection.StartAsync().Wait();
        }

        private void HandleMessage(string message)
        {
            foreach (var queue in _queues)
            {
                queue.SendAsync(message);
            }
        }

        public void Launch()
        {
            for (var i = 0; i < 10; i++)
            {
                var queue = new BufferBlock<string>();
                var someTask = new SomeTask(i, queue);

                var token = _cancellationTokenSource.Token;
                var task = Task.Run(() =>
                {
                    token.Register(() => someTask.Stop());
                    someTask.Execute();
                }, token);

                _tasks.Add(task);
                _queues.Add(queue);
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
