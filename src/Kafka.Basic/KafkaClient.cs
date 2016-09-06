﻿using System;

namespace Kafka.Basic
{
    public interface IKafkaClient : IDisposable
    {
        IKafkaTopic Topic(string name, ProducerConfig config = null);
        IKafkaConsumer Consumer(string groupName);
        IKafkaConsumer Consumer(ConsumerOptions options);
        IKafkaSimpleConsumer SimpleConsumer();
    }

    public class KafkaClient : IKafkaClient
    {
        private readonly IZookeeperConnection _zkConnection;

        public KafkaClient(string zkConnect)
        {
            _zkConnection = new ZookeeperConnection(zkConnect);
        }

        public IKafkaTopic Topic(string name, ProducerConfig config = null)
        {
            return new KafkaTopic(_zkConnection, name, config);
        }

        public IKafkaConsumer Consumer(string groupName)
        {
            return new KafkaConsumer(_zkConnection, new ConsumerOptions { GroupName = groupName });
        }

        public IKafkaConsumer Consumer(ConsumerOptions options)
        {
            return new KafkaConsumer(_zkConnection, options);
        }

        public IKafkaSimpleConsumer SimpleConsumer()
        {
            return new KafkaSimpleConsumer(_zkConnection);
        }

        public void Dispose() { }
    }
}
