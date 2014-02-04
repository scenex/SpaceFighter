// -----------------------------------------------------------------------
// <copyright file="EditorDisplay.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Editor.CustomControls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using WinFormsGraphicsDevice;

    public class EditorDisplay : GraphicsDeviceControl
    {
        private ContentManager content;
        private SpriteBatch spriteBatch;
        
        protected override void Initialize()
        {
            content = new ContentManager(Services, "Content");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
        }
    }
}
