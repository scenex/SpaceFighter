// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace SpaceFighter.Logic.StateMachine
{
    public class State<T>
    {
        internal readonly Action OnEnter;
        internal readonly Action OnExit;
        internal readonly List<Transition<T>> Transitions = new List<Transition<T>>();

        public State(string name, T tag, Action onEnter, Action onExit)
        {
            OnEnter = onEnter;
            OnExit = onExit;
            Name = name;
            Tag = tag;
        }

        public string Name { get; private set; }
        public T Tag { get; set; }

        public void AddTransition(State<T> nextState, Func<bool> condition)
        {
            Transitions.Add(new Transition<T>(nextState, condition));
        }
    }
}
