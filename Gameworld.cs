using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameJam_Jan_2025
{
    public class Gameworld : Game
    {
        //Fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //lists that are important
        private List<GameObject> activeGameObjects = new List<GameObject>();
        private List<GameObject> gameObjectsToBeAdded = new List<GameObject>();
        private List<GameObject> gameObjectsToBeRemoved = new List<GameObject>();
        private Vector2 screenSize;
        public static Vector2 MousePosition;

        //Properties

        //Constructors
        public Gameworld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        //Methods
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.ApplyChanges();
            screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach(GameObject gameObject in activeGameObjects)
            {
                gameObject.Update(gameTime, screenSize);
            }
            foreach (GameObject gameObject in gameObjectsToBeRemoved)
            {
                activeGameObjects.Remove(gameObject);
            }
            gameObjectsToBeRemoved.Clear();
            foreach(GameObject gameObject in gameObjectsToBeAdded)
            {
                activeGameObjects.Add(gameObject);
            }
            gameObjectsToBeAdded.Clear();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            foreach (GameObject gameObject in activeGameObjects)
            {
                gameObject.Draw(_spriteBatch);
            }
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        /// <summary>
        /// Gameobject will be added after next Update
        /// </summary>
        /// <param name="gameObject"></param>
        public void AddGameObject(GameObject gameObject)
        {
            gameObjectsToBeAdded.Add(gameObject);
            gameObject.LoadContent(Content);
        }

        /// <summary>
        /// Gameobject will be removed after next Update
        /// </summary>
        /// <param name="gameObject"></param>
        public void RemoveGameObject(GameObject gameObject)
        {
            gameObjectsToBeRemoved.Add(gameObject);
        }
    }
}
