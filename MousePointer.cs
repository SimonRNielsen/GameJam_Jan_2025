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
            //if (tempObject == null)
            spriteBatch.Draw(sprite, Gameworld.MousePosition, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
        }


        public void Update(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// Used to handle events for MousePointer
        /// </summary>
        /// <param name="gameTime">Gameworld logic</param>
        public void CheckCollision(GameObject gameObject)
        {

            if (!Gameworld.MouseLeftClick && !Gameworld.MouseRightClick)
                grabbed = 0;

            if (Gameworld.MouseLeftClick || Gameworld.MouseRightClick)
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
                    tempObject.Position = previousLocation;

                previousLocation = Vector2.Zero;
                tempObject = null;
            }

        }

        /// <summary>
        /// Actions to perform or not perform if left mousebutton is pressed
        /// </summary>
        private void LeftClickEvent(GameObject gameObject)
        {

            if (previousLocation == Vector2.Zero && tempObject != null)
                previousLocation = tempObject.Position;
            if (MouseOver(gameObject))
                tempObject = gameObject;
            grabbed++;
            if (tempObject is ISnapable && grabbed <= grabLimit)
            {
                tempObject.Position = new Vector2(Gameworld.MousePosition.X, Gameworld.MousePosition.Y);
            }

        }

        /// <summary>
        /// Actions to perform or not perform if right mousebutton is pressed
        /// </summary>
        private void RightClickEvent(GameObject gameObject)
        {
            if (previousLocation == Vector2.Zero && tempObject != null)
            {
                previousLocation = tempObject.Position;
                oldMouseX = Gameworld.MousePosition.X;
            }
            tempObject = gameObject;
            grabbed++;
            if (tempObject is ISnapable && grabbed <= grabLimit)
            {
                tempObject.Rotation += oldMouseX - Gameworld.MousePosition.X;
                if (tempObject.Rotation > (MathHelper.Pi * 2))
                    tempObject.Rotation -= MathHelper.Pi * 2;
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