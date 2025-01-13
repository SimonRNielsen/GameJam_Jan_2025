﻿using Microsoft.Xna.Framework;
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
            spriteBatch.Draw(sprite, Gameworld.MousePosition, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Used to handle events for MousePointer
        /// </summary>
        /// <param name="gameTime">Gameworld logic</param>
        public void Update(GameTime gameTime, GameObject gameObject)
        {
            grabbed = 0;
            if (MouseOver(gameObject))
            {
                LeftClickEvent(Gameworld.MouseLeftClick, gameObject);
                RightClickEvent(Gameworld.MouseRightClick, gameObject);
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

                tempObject = null;
            }
        }

        /// <summary>
        /// Actions to perform or not perform if left mousebutton is pressed
        /// </summary>
        /// <param name="leftClick">Checks state of left mousebutton</param>
        private void LeftClickEvent(bool leftClick, GameObject gameObject)
        {
            if (leftClick)
            {
                if (tempObject == null)
                    previousLocation = gameObject.Position;
                tempObject = gameObject;
                grabbed++;
                if (gameObject is ISnapable && grabbed <= grabLimit)
                {

                }
            }
        }

        /// <summary>
        /// Actions to perform or not perform if right mousebutton is pressed
        /// </summary>
        /// <param name="rightClick">Checks state of right mousebutton</param>
        private void RightClickEvent(bool rightClick, GameObject gameObject)
        {
            if (rightClick)
            {
                tempObject = gameObject;
                grabbed++;
                if (gameObject is ISnapable && grabbed <= grabLimit)
                {
                    gameObject.Rotation += (oldMouseX - Gameworld.MousePosition.X) / 100;
                    if (gameObject.Rotation > (MathHelper.Pi * 2))
                        gameObject.Rotation -= MathHelper.Pi * 2;
                }
            }
        }

        /// <summary>
        /// Handles collision of mouse
        /// </summary>
        /// <param name="gameObject">Facilitates interaction with object</param>
        private bool MouseOver(GameObject gameObject)
        {
            return gameObject.CollisionBox.Intersects(CollisionBox);
        }

        #endregion
    }
}