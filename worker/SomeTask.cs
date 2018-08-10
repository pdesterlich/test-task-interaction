using System;
using System.Threading.Tasks.Dataflow;

namespace worker
{
    public class SomeTask
    {
        private bool _canLoop;
        private readonly int _taskNumber;
        private BufferBlock<string> _queue;

        public SomeTask(int taskNumber, BufferBlock<string> queue)
        {
            _canLoop = true;
            _taskNumber = taskNumber;
            _queue = queue;
        }

        public void Execute()
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss} - {_taskNumber:D2} - start");

            var nextLoop = DateTime.Now;

            while (_canLoop)
            {
                if (_queue.TryReceive(out var message))
                {
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss} - {_taskNumber:D2} - incoming message: {message}");
                }

                if (DateTime.Now >= nextLoop)
                {
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss} - {_taskNumber:D2} - loop");
                    nextLoop = DateTime.Now.AddMilliseconds(5000);
                }

                System.Threading.Thread.Sleep(100);
            }

            Console.WriteLine($"{DateTime.Now:HH:mm:ss} - {_taskNumber:D2} - stop");
        }

        public void Stop()
        {
            _canLoop = false;
        }
    }
}
