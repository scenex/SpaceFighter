// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Screens
{
    public interface IScreenTransition
    {
        bool IsTransitionAllowed { get; }
        object TransitionTag { get; }
    }
}
