﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services
{
    using System;
    using Microsoft.Xna.Framework;

    public class PlayerService : GameComponent, IPlayerService
    {
        private const float MoveStep = 2.0f;
        private readonly Player player;
        private readonly Weapon weapon;

        public PlayerService(Game game) : base(game)
        {
            this.player = new Player(game, new Vector2((640 / 2) - 16, 480 / 2)); // Todo: Get screen width and height from graphics service
            game.Components.Add(this.player);

            this.weapon = new Weapon(game);
            game.Components.Add(this.weapon);
        }

        public IPlayer Player
        {
            get
            {
                return this.player;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        // Todo: Find better solution than tracking coordinates like this.
        public void MoveLeft()
        {
            this.player.Position = new Vector2(this.player.Position.X - MoveStep, this.player.Position.Y);
        }

        public void MoveRight()
        {
            this.player.Position = new Vector2(this.player.Position.X + MoveStep, this.player.Position.Y);
        }

        public void MoveUp()
        {
            this.player.Position = new Vector2(this.player.Position.X, this.player.Position.Y - MoveStep);
        }

        public void MoveDown()
        {
            this.player.Position = new Vector2(this.player.Position.X, this.player.Position.Y + MoveStep);
        }

        public void UpgradeWeapon()
        {
            throw new NotImplementedException();
        }

        public void DowngradeWeapon()
        {
            throw new NotImplementedException();
        }

        public void FireWeapon()
        {
            this.weapon.FireWeapon(new Vector2(this.player.Position.X + ((float)this.player.Sprite.Width / 2), this.player.Position.Y));
        }
    }
}
