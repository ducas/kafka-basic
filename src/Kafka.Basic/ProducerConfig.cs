namespace Kafka.Basic
{
    public class ProducerConfig
    {
        public const short DefaultAcks = 1;
        public const string DefaultClientId = "DefaultKafkaBasicClientId";
        public const int DefaultSendTimeout = 1000;

        public short Acks { get; set; }
        public string ClientId { get; set; }
        public int SendTimeout { get; set; }

        public static ProducerConfig GetDefaultConfig()
        {
            return new ProducerConfig
            {
                Acks = DefaultAcks,
                ClientId = DefaultClientId,
                SendTimeout = DefaultSendTimeout,
            };
        }

    }
}