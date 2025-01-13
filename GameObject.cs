using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam_Jan_2025
{
    public abstract class GameObject
    {
        //Fields
        protected Texture2D sprite;
        protected Vector2 position;
        protected float layer;
        //where the object/sprite originates, currently top left corner
        private Vector2 origin;
        protected float scale;
        private float rotation = 0;
        private bool removeThis = false;

        //Properties
        public Vector2 Position { get => position; set => position = value; }

        /// <summary>
        /// Handles external rotation of GameObject
        /// </summary>
        public float Rotation { get => rotation; set => rotation = value; }


        public bool RemoveThis { get => removeThis; set => removeThis = value; }

        public Rectangle CollisionBox
        {
            get { return new Rectangle((int)Gameworld.MousePosition.X, (int)Gameworld.MousePosition.Y, sprite.Width, sprite.Height); }
        }

        //Constructors
        public GameObject() { }

        //Methods
        public virtual void LoadContent(ContentManager content)
        {

        }
        public virtual void Update(GameTime gameTime, Vector2 screenSize)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, layer);
        }
    }
}
