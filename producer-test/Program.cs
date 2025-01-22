using System;
using Confluent.Kafka;

namespace producer_test
{
    class Program
    {
        public static void Main(string[] args)
        {
            var conf = new ProducerConfig { BootstrapServers = "localhost:9094" };

            Action<DeliveryReport<Null, string>> handler = r => 
                Console.WriteLine(!r.Error.IsError
                    ? $"Delivered message to {r.TopicPartitionOffset} at {DateTime.Now}"
                    : $"Delivery Error: {r.Error.Reason}");

            while (true)
            {
                var text = Console.ReadLine();
                if (text == "exit") break;

                using (var p = new ProducerBuilder<Null, string>(conf).Build())
                {
                    p.Produce("my-topic", new Message<Null, string> { Value = text }, handler);

                    // wait for up to 10 seconds for any inflight messages to be delivered.
                    p.Flush(TimeSpan.FromSeconds(10));
                }
            }
        }
    }
}
