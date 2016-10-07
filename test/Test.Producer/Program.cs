using System;
using System.Linq;
using System.Threading;
using CommandLine;
using Kafka.Basic;
using Metrics;
using System.Text;

namespace Producer
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default
                .ParseArguments<Options>(args)
                .MapResult(
                    Run,
                    errs => 1
                );
        }
        private static string CreateStringOfSize(int size)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
                sb.Append("a");
            return sb.ToString();
        }
        private static int Run(Options options)
        {
            var timer = Metric.Timer("Sent", Unit.Events);
            Metric.Config.WithReporting(r => r.WithConsoleReport(TimeSpan.FromSeconds(5)));
            var msg = CreateStringOfSize(300);
            var client = new KafkaClient(options.ZkConnect);
            var topic = client.Topic(options.Topic);

            var published = 0;
            var stop = false;
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                stop = true;
            };

            var start = DateTime.Now;

            while (!stop)
            {
                Thread.Sleep(options.Sleep);
                var batch =
                    Enumerable.Range(0, options.BatchSize)
                        .Select(i =>
                            new Message
                            {
                                Key = (published + i).ToString(),
                                Value = msg + i.ToString()
                            })
                        .ToArray();
                var time = DateTime.UtcNow.Ticks;
                topic.Send(batch);
                var diff = (DateTime.UtcNow.Ticks - time) / 10000;
                timer.Record(diff, TimeUnit.Milliseconds);

                published += options.BatchSize;
                if (published >= options.Messages) break;
            }
            var end = DateTime.Now;
            var timeTake = (end - start).TotalSeconds;

            Console.WriteLine("Published {0} messages in {1} seconds", published, timeTake);

            return 0;
        }
    }
}
