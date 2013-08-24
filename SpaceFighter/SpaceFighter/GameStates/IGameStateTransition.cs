// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.GameStates
{
    public interface IGameStateTransition
    {
        bool IsTransitionAllowed { get; }
        object TransitionTag { get; }
    }
}
