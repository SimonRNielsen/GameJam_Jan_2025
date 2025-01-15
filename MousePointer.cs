using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam_Jan_2025
{
    internal class MousePointer
    {
        #region Fields

        Texture2D sprite;
        private float oldMouseX;
        private float Pi = MathHelper.Pi;
        GameObject tempObject;
        Vector2 previousLocation;
        private bool leftButtonClicked;
        private bool mouseClicked;
        private bool itemTrashed;

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
        /// Handles input and movement of objects
        /// </summary>
        public void Update()
        {

            if (!Gameworld.MouseLeftClick && !Gameworld.MouseRightClick)
            {

                CheckCollision(tempObject);

                if (leftButtonClicked && mouseClicked)
                {
                    mouseClicked = false;
                    if (itemTrashed)
                    {
                        itemTrashed = false;
                        Gameworld.sounds["trashSound"].Play();
                    }
                    else
                        Gameworld.sounds["moveSound"].Play();
                }
                else if (!leftButtonClicked && mouseClicked)
                {
                    mouseClicked = false;
                    Gameworld.sounds["rotateSound"].Play();
                }

            }

            if (tempObject != null && tempObject.Grabbed)
            {
                if (Gameworld.MouseLeftClick)
                {
                    tempObject.Position = Gameworld.MousePosition;
                    leftButtonClicked = true;
                    mouseClicked = true;
                }
                else if (Gameworld.MouseRightClick)
                {
                    leftButtonClicked = false;
                    mouseClicked = true;
                    tempObject.Rotation += (Gameworld.MousePosition.X - oldMouseX) / 100f;
                    oldMouseX = Gameworld.MousePosition.X;
                    if (tempObject.Rotation > (MathHelper.Pi * 2))
                        tempObject.Rotation -= MathHelper.Pi * 2;
                    if (tempObject.Rotation < -(MathHelper.Pi * 2))
                        tempObject.Rotation += MathHelper.Pi * 2;
                }
            }

        }

        /// <summary>
        /// Used to handle events for MousePointer
        /// </summary>
        /// <param name="gameObject">Object to be manipulated</param>
        public void CheckCollision(GameObject gameObject)
        {

            if (Gameworld.MouseLeftClick || Gameworld.MouseRightClick)
            {
                if (Gameworld.MouseLeftClick)
                    LeftClickEvent(gameObject);
                else
                    RightClickEvent(gameObject);
            }

            if (!Gameworld.MouseRightClick && !Gameworld.MouseLeftClick && tempObject != null)
            {

                //Rotation end logic
                if (tempObject.Rotation < Pi / 4 && tempObject.Rotation > 0 || tempObject.Rotation > (Pi / 4) * 7)
                    tempObject.Rotation = 0;
                else if (tempObject.Rotation > (Pi / 4) * 1 && tempObject.Rotation < (Pi / 4) * 3)
                    tempObject.Rotation = Pi / 2;
                else if (tempObject.Rotation > (Pi / 4) * 3 && tempObject.Rotation < (Pi / 4) * 5)
                    tempObject.Rotation = Pi;
                else if (tempObject.Rotation > (Pi / 4) * 5 && tempObject.Rotation < (Pi / 4) * 7)
                    tempObject.Rotation = (Pi / 2) * 3;
                else if (tempObject.Rotation < -(Pi / 4) * 1 && tempObject.Rotation > -(Pi / 4) * 3)
                    tempObject.Rotation = (Pi / 2) * 3;
                else if (tempObject.Rotation < -(Pi / 4) * 3 && tempObject.Rotation > -(Pi / 4) * 5)
                    tempObject.Rotation = Pi;
                else if (tempObject.Rotation < -(Pi / 4) * 5 && tempObject.Rotation > -(Pi / 4) * 7)
                    tempObject.Rotation = Pi / 2;
                else
                    tempObject.Rotation = 0;

                //"Drop" logic
                bool intersection = false;

                foreach (Rectangle rectangle in Gameworld.snapBoard.parts.Keys)
                {
                    if (tempObject.CollisionBox.Intersects(rectangle))
                    {
                        intersection = true;
                        break;
                    }
                }

                bool success = false;

                if (intersection)
                    success = Gameworld.snapBoard.UpdateSlot(tempObject);
                else
                    tempObject.Position = previousLocation;
                if (!success)
                    tempObject.Position = previousLocation;
                if (success)
                {
                    if (tempObject.Position == Gameworld.snapBoard.partsPositions[Gameworld.snapBoard.Trashcan])
                        itemTrashed = true;
                    else
                        itemTrashed = false;
                    Gameworld.conveyorBelt.RemoveFromAllParts(tempObject);
                }

                //Reset check parameters and clear tempObject
                previousLocation = Vector2.Zero;
                tempObject.Grabbed = false;
                Gameworld.Grabbing = false;
                tempObject = null;
            }

        }

        /// <summary>
        /// Actions to perform or not perform if left mousebutton is pressed
        /// </summary>
        private void LeftClickEvent(GameObject gameObject)
        {

            if (MouseOver(gameObject))
                if (gameObject is ISnapable)
                {
                    Gameworld.Grabbing = true;
                    tempObject = gameObject;
                    tempObject.Grabbed = true;
                }
            if (previousLocation == Vector2.Zero && tempObject != null)
                previousLocation = tempObject.Position;
        }

        /// <summary>
        /// Actions to perform or not perform if right mousebutton is pressed
        /// </summary>
        private void RightClickEvent(GameObject gameObject)
        {
            if (MouseOver(gameObject))
                if (gameObject is ISnapable)
                {
                    Gameworld.Grabbing = true;
                    tempObject = gameObject;
                    tempObject.Grabbed = true;
                }
            if (previousLocation == Vector2.Zero && tempObject != null)
            {
                previousLocation = tempObject.Position;
                oldMouseX = Gameworld.MousePosition.X;
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