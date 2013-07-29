// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.EventManager
{
    using System.Collections.Generic;
    using System.Linq;

    public class EventAggregator
    {
        static readonly List<Subscriber> Subscribers = new List<Subscriber>();
        
        public static void Subscribe(object sender, string topic)
        {
            var subscriberMethods = 
                sender.GetType()
                      .GetMethods()
                      .Where(methodInfo => methodInfo.GetCustomAttributes(typeof(SubscriptionAttribute), false).Length > 0)
                      .ToArray();

            foreach (var subscriberMethod in subscriberMethods)
            {
                var subscriptionAttributes = subscriberMethod.GetCustomAttributes(typeof(SubscriptionAttribute), false).Cast<SubscriptionAttribute>();
                
                if (subscriptionAttributes.Any(memberInfo => memberInfo.Topic == topic))
                {
                    Subscribers.Add(new Subscriber(sender, topic, subscriberMethod));
                }
            }
        }

        public static void Unsubscribe(object sender, string topic)
        {
            Subscribers.RemoveAll2(subscriber => subscriber.Sender == sender && subscriber.Topic == topic);
        }

        public static void Fire(object sender, string topic)
        {
            var interestedSubscribers = Subscribers.Where(subscriber => subscriber.Topic == topic).ToArray();

            foreach (var interestedSubscriber in interestedSubscribers)
            {
                interestedSubscriber.MethodToCall.Invoke(interestedSubscriber.Sender, null);
            }
        }
    }
}