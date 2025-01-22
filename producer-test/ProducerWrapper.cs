using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace producer_test
{
    public class ProducerWrapper : IDisposable
    {
        private string _topicName;
        private IProducer<string,string> _producer;
        private ProducerConfig _config;

        public ProducerWrapper(ProducerConfig config, string topicName)
        {
            _topicName = topicName;
            _config = config;
            _producer = new ProducerBuilder<string, string>(_config).Build();
        }

        public async Task WriteMessage(string message)
        {
            var dr = await _producer.ProduceAsync(_topicName, new Message<string, string> { Key="adsdad123", Value=message });  
            //return Task.CompletedTask;
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}