using System;
using Confluent.Kafka;

namespace producer_test
{
    class Program
    {
        public static void Main(string[] args)
        {
            var conf = new ProducerConfig { BootstrapServers = "localhost:9094" };

            Action<DeliveryReport<Null, Pnd.Streams.EventLog>> handler = r => 
                Console.WriteLine(!r.Error.IsError
                    ? $"Delivered message to {r.TopicPartitionOffset} at {DateTime.Now}"
                    : $"Delivery Error: {r.Error.Reason}");

            while (true)
            {
                var text = Console.ReadLine();
                if (text == "exit") break;

                var eventLog = new Pnd.Streams.EventLog 
                {
                    Id = Guid.NewGuid().ToString(),
                    Subject = "Entity_UPDATE",
                    Body = text
                };

                using (var p = new ProducerBuilder<Null, Pnd.Streams.EventLog>(conf)
                    .SetValueSerializer(new ProtoSerializer<Pnd.Streams.EventLog>()).Build())
                {
                    p.Produce("my-topic", new Message<Null, Pnd.Streams.EventLog> { Value = eventLog }, handler);

                    // wait for up to 10 seconds for any inflight messages to be delivered.
                    p.Flush(TimeSpan.FromSeconds(10));
                }
            }
        }
    }
}
