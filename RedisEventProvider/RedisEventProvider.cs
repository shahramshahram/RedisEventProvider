using System;
using System.Threading.Tasks;
using ServiceStack.Redis;
using EPiServer.Events.Providers;
using EPiServer.Events;

namespace EPiServer.Redis.Events
{
  
    public class RedisEventProvider : EventProvider
    {
        private RedisClient _client;
        private IRedisSubscription _subscription;
        private string _channelName;
        private string _hostName;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            _channelName = config["channelName"];
            if (String.IsNullOrEmpty(_channelName))
            {
                throw new ApplicationException("The connection to the redis service should have a valid channelName");
            }

            _hostName = config["hostName"];
            if (String.IsNullOrEmpty(_hostName))
            {
                throw new ApplicationException("The connection to the redis service should have a valid hostName");
            }
            _client = new RedisClient(_hostName);
        }

        public override Task InitializeAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                CreateSubscription(_channelName);
            });
        }

        public override void Uninitialize()
        {
            _subscription.UnSubscribeFromAllChannels();
            base.Uninitialize();
        }
 
        public override void SendMessage(EventMessage message)
        {
            _client.PublishMessage(_channelName, JsonConverter.ToJson<EventMessage>(message));
        }

        void CreateSubscription(string channelName)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var client = new RedisClient(_hostName))
                    {
                        _subscription = client.CreateSubscription();
                        _subscription.OnMessage = (channel, msg) =>
                        {
                            if (channel == _channelName)
                            {
                                try
                                {
                                    OnMessageReceived(new EventMessageEventArgs(JsonConverter.FromJson<EventMessage>(msg)));
                                }
                                catch
                                {
                                    //log
                                }
                            }
                        };
                        _subscription.SubscribeToChannels(channelName);
                    }
                }
                catch
                {
                    //log
                }
            });
        }
    }

}
