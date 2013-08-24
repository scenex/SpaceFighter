﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Screens
{
    public class GameplayScreen : IScreenTransition
    {
        public bool IsTransitionAllowed { get; private set; }
        public object TransitionTag { get; private set; }
    }
}
