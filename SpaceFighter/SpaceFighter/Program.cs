// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter
{
    using System;

    using Microsoft.Xna.Framework;

    using Ninject;

    using SpaceFighter.Logic;
    using SpaceFighter.Logic.Services.Implementations;
    using SpaceFighter.Logic.Services.Interfaces;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel();

            kernel.Bind<ISpaceGame>().To<SpaceGame>();

            //kernel.Bind<GraphicsDeviceManager>().ToSelf();

            kernel.Bind<IServiceProvider>().To<ServiceProviderWrapper>();

            kernel.Bind<IGameController>().To<GameController>();
            kernel.Bind<IInputService>().To<InputService>();
            kernel.Bind<ICollisionDetectionService>().To<CollisionDetectionService>();
            kernel.Bind<IPlayerService>().To<PlayerService>();
            kernel.Bind<IEnemyService>().To<EnemyService>();
            kernel.Bind<ITerrainService>().To<TerrainService>();
            kernel.Bind<ICameraService>().To<CameraService>();
            kernel.Bind<IHeadUpDisplayService>().To<HeadUpDisplayService>();
            kernel.Bind<IAudioService>().To<AudioService>();
            kernel.Bind<IDebugService>().To<DebugService>();

            var spacegame = kernel.Get<ISpaceGame>() as SpaceGame;

            using (spacegame)
            {
                if (spacegame != null)
                {
                    spacegame.Run();
                }
            }
        }
    }
}

