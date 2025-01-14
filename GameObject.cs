using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
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
        protected Vector2 origin;
        protected float scale = 1;
        protected Color color = Color.White;
        private float rotation;
        private bool removeThis = false;
        private bool grabbed = false;

        //Properties
        public Vector2 Position { get => position; set => position = value; }

        /// <summary>
        /// Handles external rotation of GameObject
        /// </summary>
        public float Rotation { get => rotation; set => rotation = value; }


        public bool RemoveThis { get => removeThis; set => removeThis = value; }


        public bool Grabbed { get => grabbed; set => grabbed = value; }

        public Rectangle CollisionBox
        {

            get
            {
                if (sprite != null)
                    return new Rectangle((int)(Position.X - (sprite.Width / 2) * scale), (int)(Position.Y - (sprite.Height / 2) * scale), (int)(sprite.Width * scale), (int)(sprite.Height * scale));
                else 
                    return new Rectangle();
            }
        }

        //Constructors
        public GameObject() { }

        //Methods
        public virtual void LoadContent(ContentManager content)
        {

        }
        public virtual void Update(GameTime gameTime)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (sprite != null)
                spriteBatch.Draw(sprite, position, null, Color.White, rotation, new Vector2(sprite.Width / 2, sprite.Height / 2), scale, SpriteEffects.None, layer);
        }
    }
}
