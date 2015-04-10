# RedisEventProvider
EPiServer Reddis Event Provider base on ServiceStack.Redis
Example of Webconfig

```xml
  <episerver.framework>
    ...
    <event defaultProvider="redisevents">
      <providers>
        <add name="redisevents" type="EPiServer.Redis.Events.RedisEventProvider, RedisEventProvider"
              channelName="EPiServerRedis" hostName="127.0.0.1"/>
      </providers>
    </event>
  </episerver.framework>
```

