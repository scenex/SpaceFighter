﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Services.Interfaces;

    /// <summary>
    /// Todo: Level drawing happening here in GameController, is it the right place?
    /// </summary>
    public class GameController : DrawableGameComponent, IGameController
    {
        public IAudioService AudioService { get; set; }

        private SpriteBatch spriteBatch;
        private readonly List<Texture2D> spriteList = new List<Texture2D>();

        private ICollisionDetectionService collisionDetectionService;
        private IPlayerService playerService;
        private IEnemyService enemyService;
        private IInputService inputService;

        private readonly IHeadUpDisplayService headUpDisplayService;

        private ITerrainService terrainService;
        private IDebugService debugService;
        private IAudioService audioService;
        private ICameraService cameraService;

        public GameController(
            Game game,
            ICollisionDetectionService collisionDetectionService,
            IPlayerService playerService,
            IEnemyService enemyService,
            IInputService inputService,
            IHeadUpDisplayService headUpDisplayService,
            ITerrainService terrainService,
            IDebugService debugService,
            IAudioService audioService,
            ICameraService cameraService) : base(game)
        {
            this.AudioService = audioService;
            this.collisionDetectionService = collisionDetectionService;
            this.playerService = playerService;
            this.enemyService = enemyService;
            this.inputService = inputService;
            this.headUpDisplayService = headUpDisplayService;
            this.terrainService = terrainService;
            this.debugService = debugService;
            this.cameraService = cameraService;
        }

        public override void Initialize()
        {
            this.collisionDetectionService.EnemyHit += this.OnEnemyHit;
            this.collisionDetectionService.PlayerHit += this.OnPlayerHit;
            this.collisionDetectionService.PlayerEnemyHit += this.OnPlayerEnemyHit;
            this.collisionDetectionService.BoundaryHit += this.OnBoundaryHit;

            this.playerService.TransitionToStateDying += this.OnTransitionToStateDying;
            this.playerService.TransitionToStateDead += this.OnTransitionToStateDead;
            this.playerService.TransitionToStateRespawn += this.OnTransitionToStateRespawn;
            this.playerService.TransitionToStateAlive += this.OnTransitionToStateAlive;
            this.playerService.HealthChanged += this.OnHealthChanged;

            // DISABLE MUSIC WHILE DEVELOPMENT
            // this.audioService.PlaySound("music2");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            var tileList = this.Game.Content.Load<List<string>>("manifest").Where(x => x.StartsWith(@"Sprites\L1\")).ToList();

            foreach (var tile in tileList)
            {
                spriteList.Add(this.Game.Content.Load<Texture2D>(tile));
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdatePlayerPositionForEnemies();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // this.debugService.DrawRectangle(new Rectangle(((int)playerService.Player.Position.X / 80) * 80, ((int)playerService.Player.Position.Y / 80) * 80, 80, 80));
            
            this.spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                cameraService.GetTransformation());

            for (int i = 0; i < this.terrainService.VerticalTileCount; i++)
            {
                for (int j = 0; j < this.terrainService.HorizontalTileCount; j++)
                {
                    this.spriteBatch.Draw(this.spriteList[this.terrainService.Map[i, j]], new Vector2(j * this.terrainService.TileSize, i * this.terrainService.TileSize), Color.White);
                }
            }

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdatePlayerPositionForEnemies()
        {
            foreach (var enemy in this.enemyService.Enemies.ToList())
            {
                enemy.PlayerPosition = this.playerService.Player.Position;
            }
        }

        private void OnTransitionToStateDying(object sender, EventArgs eventArgs)
        {   
            this.inputService.Disable();
            this.collisionDetectionService.Disable();  
        }

        private void OnTransitionToStateDead(object sender, EventArgs eventArgs)
        {
            // Continue...
        }

        private void OnTransitionToStateRespawn(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            this.inputService.Enable();
        }

        private void OnTransitionToStateAlive(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            this.collisionDetectionService.Enable();
        }

        private void OnHealthChanged(object sender, HealthChangedEventArgs healthChangedEventArgs)
        {
            this.headUpDisplayService.Health = healthChangedEventArgs.NewHealth;
        }

        private void OnEnemyHit(object sender, EnemyHitEventArgs e)
        {
            this.playerService.RemoveShot(e.Shot);
            this.enemyService.ReportEnemyHit(e.Enemy, e.Shot);
        }

        private void OnPlayerHit(object sender, PlayerHitEventArgs e)
        {
            this.enemyService.RemoveShot(e.Shot);
            this.playerService.ReportPlayerHit(e.Shot);
            this.headUpDisplayService.Health = this.playerService.Player.Health;
        }

        private void OnPlayerEnemyHit(object sender, EventArgs e)
        {
            this.playerService.ReportPlayerHit(100);
            this.headUpDisplayService.Health = this.playerService.Player.Health;
        }

        private void OnBoundaryHit(object sender, EventArgs e)
        {
            this.playerService.ReportPlayerHit(100);
            this.headUpDisplayService.Health = this.playerService.Player.Health;
        }
    }
}
