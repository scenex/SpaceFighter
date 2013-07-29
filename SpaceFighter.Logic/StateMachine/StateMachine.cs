// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.StateMachine
{
    /// <summary>
    /// State machine from:
    /// http://astroboid.com/2011/05/xna-ai-finite-state-machine.html
    /// </summary>
    public class StateMachine<T>
    {
        public StateMachine(State<T> currentState)
        {
            CurrentState = currentState;

            if (CurrentState.OnEnter != null)
            {
                CurrentState.OnEnter();
            }
        }

        public State<T> CurrentState { get; private set; }

        public void Update()
        {
            while (MoveToNext()) { }
        }

        private bool MoveToNext()
        {
            for (int i = 0; i < CurrentState.Transitions.Count; i++)
            {
                Transition<T> t = CurrentState.Transitions[i];

                if (t.Condition())
                {
                    if (CurrentState.OnExit != null)
                    {
                        CurrentState.OnExit();
                    }
                        
                    CurrentState = t.NextState;

                    if (CurrentState.OnEnter != null)
                    {
                        CurrentState.OnEnter();
                    }
                        
                    return true;
                }
            }

            return false;
        }
    }
}
