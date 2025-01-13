using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace GameJam_Jan_2025
{
    public class Gameworld : Game
    {
        //Fields
        private static ContentManager Content2;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static List<GameObject> activeGameObjects = new List<GameObject>();
        private static List<GameObject> gameObjectsToBeAdded = new List<GameObject>();
        private static List<GameObject> gameObjectsToBeRemoved = new List<GameObject>();
        private Vector2 screenSize;
        public static Vector2 MousePosition;
        private static bool mouseLeftClick;
        private static bool mouseRightClick;
        private static MousePointer mousePointer;
        public static Dictionary<string, Texture2D> sprites = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D[]> animations = new Dictionary<string, Texture2D[]>();
        public static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        public static Dictionary<string, Song> music = new Dictionary<string, Song>();

        //Properties
        public static bool MouseLeftClick { get => mouseLeftClick; }
        public static bool MouseRightClick { get => mouseRightClick; }

        //Constructors
        public Gameworld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Content2 = Content;
            IsMouseVisible = false;
        }

        //Methods
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.ApplyChanges();
            screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            AddSprites(sprites);
            AddAnimation(animations);
            AddSounds(sounds);
            AddMusic(music);

            mousePointer = new MousePointer();

            AddGameObject(new Timer());

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
                gameObject.Update(gameTime);
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

            var mouseState = Mouse.GetState();
            MousePosition = new Vector2(mouseState.Position.X, mouseState.Position.Y);
            mouseLeftClick = mouseState.LeftButton == ButtonState.Pressed;
            mouseRightClick = mouseState.RightButton == ButtonState.Pressed;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            foreach (GameObject gameObject in activeGameObjects)
            {
                gameObject.Draw(_spriteBatch);
            }
            
            mousePointer.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }


        private void AddSprites(Dictionary<string, Texture2D> sprites)
        {

            Texture2D mouse = Content.Load<Texture2D>("Sprites\\Mouse\\screwdriver_mousepointer");
            sprites.Add("mouse", mouse);

            Texture2D timerBackground = Content.Load<Texture2D>("Sprites\\Timer\\basic timer background");
            sprites.Add("timerBackground", timerBackground);

            Texture2D timerForeground = Content.Load<Texture2D>("Sprites\\Timer\\basic timer foreground");
            sprites.Add("timerForeground", timerForeground);
        }

        private void AddAnimation(Dictionary<string, Texture2D[]> animations)
        {

        }

        private void AddSounds(Dictionary<string, SoundEffect> sounds)
        {

        }

        private void AddMusic(Dictionary<string, Song> music)
        {

        }

        /// <summary>
        /// Gameobject will be added after next Update
        /// </summary>
        /// <param name="gameObject"></param>
        public static void AddGameObject(GameObject gameObject)
        {
            gameObjectsToBeAdded.Add(gameObject);
            gameObject.LoadContent(Content2);
        }

        /// <summary>
        /// Gameobject will be removed after next Update
        /// </summary>
        /// <param name="gameObject"></param>
        public static void RemoveGameObject(GameObject gameObject)
        {
            gameObjectsToBeRemoved.Add(gameObject);
        }
    }
}
