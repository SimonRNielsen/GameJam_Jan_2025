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
        protected Vector2 origin;
        protected float scale=1;
        protected Color color = Color.White;

        //Properties
        public Vector2 Position { get => position; set => position = value; }


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
            spriteBatch.Draw(sprite, position, null, color, 0, origin, scale, SpriteEffects.None, layer);
        }
    }
}
