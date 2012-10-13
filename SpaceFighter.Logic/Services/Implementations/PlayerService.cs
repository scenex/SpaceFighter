// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Services.Interfaces;

    public class PlayerService : GameComponent, IPlayerService
    {
        private const float MoveStep = 2.0f;
        private Player player;

        private IWeaponService weaponService;


        public PlayerService(Game game) : base(game)
        {
        }

        public IPlayer Player
        {
            get
            {
                return this.player;
            }
        }

        public override void Initialize()
        {
            this.weaponService = (IWeaponService)this.Game.Services.GetService(typeof(IWeaponService));
            
            this.player = new Player(this.Game, new Vector2((this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 16, (this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 0));
            this.Game.Components.Add(this.player);

            base.Initialize();
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

        public void Fire()
        {
            this.weaponService.FireWeapon(new Vector2(this.player.Position.X + ((float)this.player.Sprite.Width / 2), this.player.Position.Y));
        }
    }
}
