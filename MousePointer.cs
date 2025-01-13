using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam_Jan_2025
{
    internal class MousePointer
    {
        #region Fields

        Texture2D sprite;

        #endregion

        #region Properties

        /// <summary>
        /// Returns a 1x1 pixel CollisionBox at the tip of the mousepointer
        /// </summary>
        public Rectangle CollisionBox
        {
            get { return new Rectangle((int)Gameworld.MousePosition.X, (int)Gameworld.MousePosition.Y, 1, 1); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a MousePointer for interacting with objects
        /// </summary>
        public MousePointer()
        {
            sprite = Gameworld.sprites["mouse"];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws a custom mousecursor at the location its detected to be in
        /// </summary>
        /// <param name="spriteBatch">Gameworld logic</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Gameworld.MousePosition, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
        }

        #endregion
    }
}