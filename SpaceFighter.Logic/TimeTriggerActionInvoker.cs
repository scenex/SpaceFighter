// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;

    public class TimeTriggerActionInvoker
    {
        private readonly Action triggerAction;
        private readonly double triggerMilliseconds;

        public TimeTriggerActionInvoker(double triggerMilliseconds, Action triggerAction)
        {
            this.triggerMilliseconds = triggerMilliseconds;
            this.triggerAction = triggerAction;
        }

        public double TriggerMilliseconds
        {
            get
            {
                return this.triggerMilliseconds;
            }
        }

        public void TriggerAction()
        {
            this.triggerAction.Invoke();
        }
    }
}
