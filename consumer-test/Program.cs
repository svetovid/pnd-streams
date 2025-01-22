using System;
using System.Diagnostics;
using System.Threading;
using Confluent.Kafka;

namespace consumer_test
{
    class Program
    {
        public static void Main(string[] args)
        {
            var conf = new ConsumerConfig
            { 
                GroupId = "test-consumer-group",
                BootstrapServers = "kafka:9092",
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
             
            using (var c = new ConsumerBuilder<Ignore, Pnd.Streams.EventLog>(conf)
                .SetValueDeserializer(new ProtoDeserializer<Pnd.Streams.EventLog>()).Build())
            {
                c.Subscribe("my-topic");

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                try
                {
                    while (true)
                    {
                        try
                        {
                            // await Task.Delay(1000);

                            var cr = c.Consume(cts.Token);
                            Console.WriteLine($"Consumed message '{cr.Value.Body}' with subject '{cr.Value.Subject}' at: '{cr.TopicPartitionOffset}'. DateTime: {DateTime.Now}");

                            if (stopwatch.ElapsedMilliseconds > 20000)
                            {
                                break;
                            }
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                finally
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
            }
        }
    }
}
