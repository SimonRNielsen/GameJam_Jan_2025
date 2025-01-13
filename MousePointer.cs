using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam_Jan_2025
{
    internal class MousePointer
    {
        #region Fields

        Texture2D sprite;
        private float rotation;
        private int grabLimit = 1;
        private int grabbed;
        private float oldMouseX;
        private float oldMouseY;
        GameObject tempObject;
        Vector2 previousLocation;

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
            if (tempObject == null)
                spriteBatch.Draw(sprite, Gameworld.MousePosition, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Used to handle events for MousePointer
        /// </summary>
        /// <param name="gameTime">Gameworld logic</param>
        public void CheckCollision(GameObject gameObject)
        {

            if (!Gameworld.MouseLeftClick && !Gameworld.MouseRightClick)
                grabbed = 0;

            if (Gameworld.MouseLeftClick && MouseOver(gameObject) || Gameworld.MouseRightClick && MouseOver(gameObject))
            {
                if (Gameworld.MouseLeftClick)
                    LeftClickEvent(gameObject);
                if (Gameworld.MouseRightClick)
                    RightClickEvent(gameObject);
            }

            if (!Gameworld.MouseRightClick && !Gameworld.MouseLeftClick && tempObject != null)
            {

                if (tempObject.Rotation < MathHelper.Pi / 4 || tempObject.Rotation > (MathHelper.Pi / 4) * 7)
                    tempObject.Rotation = 0;
                else if (tempObject.Rotation > (MathHelper.Pi / 4) * 1 && tempObject.Rotation < (MathHelper.Pi / 4) * 3)
                    tempObject.Rotation = MathHelper.Pi / 2;
                else if (tempObject.Rotation > (MathHelper.Pi / 4) * 3 && tempObject.Rotation < (MathHelper.Pi / 4) * 5)
                    tempObject.Rotation = MathHelper.Pi;
                else if (tempObject.Rotation > (MathHelper.Pi / 4) * 5 && tempObject.Rotation < (MathHelper.Pi / 4) * 7)
                    tempObject.Rotation = (MathHelper.Pi / 2) * 3;

                bool intersection = false;

                foreach (Rectangle rectangle in Gameworld.snapBoard.Slots)
                {
                    if (tempObject.CollisionBox.Intersects(rectangle))
                    {
                        intersection = true;
                        break;
                    }
                }

                if (intersection)
                    Gameworld.snapBoard.UpdateSlot(tempObject);
                else
                    gameObject.Position = previousLocation;

                tempObject = null;
            }

        }

        /// <summary>
        /// Actions to perform or not perform if left mousebutton is pressed
        /// </summary>
        /// <param name="leftClick">Checks state of left mousebutton</param>
        private void LeftClickEvent(GameObject gameObject)
        {

            if (tempObject == null)
                previousLocation = gameObject.Position;
            tempObject = gameObject;
            grabbed++;
            if (gameObject is ISnapable && grabbed <= grabLimit)
            {
                gameObject.Position = Gameworld.MousePosition;
            }

        }

        /// <summary>
        /// Actions to perform or not perform if right mousebutton is pressed
        /// </summary>
        /// <param name="rightClick">Checks state of right mousebutton</param>
        private void RightClickEvent(GameObject gameObject)
        {
            if (tempObject == null)
                oldMouseX = Gameworld.MousePosition.X;
            tempObject = gameObject;
            grabbed++;
            if (gameObject is ISnapable && grabbed <= grabLimit)
            {
                gameObject.Rotation += (oldMouseX - Gameworld.MousePosition.X) / 100;
                if (gameObject.Rotation > (MathHelper.Pi * 2))
                    gameObject.Rotation -= MathHelper.Pi * 2;
            }

        }

        /// <summary>
        /// Handles collision of mouse
        /// </summary>
        /// <param name="gameObject">Facilitates interaction with object</param>
        public bool MouseOver(GameObject gameObject)
        {
            return gameObject.CollisionBox.Intersects(CollisionBox);
        }

        #endregion
    }
}