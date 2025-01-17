using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace GameJam_Jan_2025
{
    public abstract class Part:GameObject
    {

        protected int partType;

        /// <summary>
        /// For identifying part type for score
        /// </summary>
        public int PartType { get => partType; }

        public Part() 
        {
            layer = 0.5f;
        }

        //Methods
        public override void LoadContent(ContentManager content)
        {

        }
        public override void Update(GameTime gameTime)
        {

        }
    }
}
