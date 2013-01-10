// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.StateMachine
{
    using System;

    internal class Transition<T>
    {
        internal readonly Func<bool> Condition;
        internal readonly State<T> NextState;

        internal Transition(State<T> nextState, Func<bool> condition)
        {
            NextState = nextState;
            Condition = condition;
        }
    }
}
