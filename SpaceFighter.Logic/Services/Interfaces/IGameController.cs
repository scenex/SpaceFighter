﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    public interface IGameController
    {
        // Todo: Remove from interface -> encapsulate
        IPlayerService PlayerService { get; }
    }
}
