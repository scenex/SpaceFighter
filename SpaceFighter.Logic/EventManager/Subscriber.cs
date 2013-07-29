// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.EventManager
{
    using System.Reflection;

    public class Subscriber
    {
        public string Topic { get; set; }

        public MethodInfo MethodToCall { get; set; }

        public object Sender { get; set; }

        public Subscriber(object sender, string topic, MethodInfo methodToCall)
        {
            Sender = sender;
            Topic = topic;
            MethodToCall = methodToCall;
        }
    }
}
