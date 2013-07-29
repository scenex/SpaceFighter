// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.EventManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SubscriptionAttribute : Attribute
    {
        public string Topic { get; set; }

        public SubscriptionAttribute(string topic)
        {
            Topic = topic;
        }
    }
}
