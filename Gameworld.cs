using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private static bool mouseLeftClick;
        private static bool mouseRightClick;
        private static MousePointer mousePointer;
        public static Dictionary<string, Texture2D> sprites = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D[]> animations = new Dictionary<string, Texture2D[]>();
        public static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        public static Dictionary<string, Song> music = new Dictionary<string, Song>();

        public static bool MouseLeftClick { get => mouseLeftClick; }
        public static bool MouseRightClick { get => mouseRightClick; }

        //Properties

        //Constructors
        public Gameworld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            activeGameObjects.Add(new Head());    

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

            #region parts
            Texture2D robotHead1 = Content.Load<Texture2D>("Sprites\\Robotparts\\head1");
            Texture2D robotHead2 = Content.Load<Texture2D>("Sprites\\Robotparts\\head1");
            Texture2D robotHead3 = Content.Load<Texture2D>("Sprites\\Robotparts\\head1");

            Texture2D robotBody1 = Content.Load<Texture2D>("Sprites\\Robotparts\\body1");
            Texture2D robotBody2 = Content.Load<Texture2D>("Sprites\\Robotparts\\body2");
            Texture2D robotBody3 = Content.Load<Texture2D>("Sprites\\Robotparts\\body3");

            Texture2D robotArmL1 = Content.Load<Texture2D>("Sprites\\Robotparts\\armL1");
            Texture2D robotArmL2 = Content.Load<Texture2D>("Sprites\\Robotparts\\armL2");
            Texture2D robotArmL3 = Content.Load<Texture2D>("Sprites\\Robotparts\\armL3");

            Texture2D robotArmR1 = Content.Load<Texture2D>("Sprites\\Robotparts\\armR1");
            Texture2D robotArmR2 = Content.Load<Texture2D>("Sprites\\Robotparts\\armR2");
            Texture2D robotArmR3 = Content.Load<Texture2D>("Sprites\\Robotparts\\armR3");

            Texture2D robotLegL1 = Content.Load<Texture2D>("Sprites\\Robotparts\\legL1");
            Texture2D robotLegL2 = Content.Load<Texture2D>("Sprites\\Robotparts\\legL2");
            Texture2D robotLegL3 = Content.Load<Texture2D>("Sprites\\Robotparts\\legL3");

            Texture2D robotLegR1 = Content.Load<Texture2D>("Sprites\\Robotparts\\legR1");
            Texture2D robotLegR2 = Content.Load<Texture2D>("Sprites\\Robotparts\\legR2");
            Texture2D robotLegR3 = Content.Load<Texture2D>("Sprites\\Robotparts\\legR3");

            sprites.Add("head1", robotHead1); 
            sprites.Add("head2", robotHead2); 
            sprites.Add("head3", robotHead3);

            sprites.Add("robotBody1", robotBody1);
            sprites.Add("robotBody2", robotBody2);
            sprites.Add("robotBody3", robotBody3);

            sprites.Add("robotArmL1", robotArmL1);
            sprites.Add("robotArmL2", robotArmL2);
            sprites.Add("robotArmL3", robotArmL3);

            sprites.Add("robotArmR1", robotArmR1);
            sprites.Add("robotArmR2", robotArmR2);
            sprites.Add("robotArmR3", robotArmR3);

            sprites.Add("robotLegL1", robotLegL1);
            sprites.Add("robotLegL2", robotLegL2);
            sprites.Add("robotLegL3", robotLegL3);

            sprites.Add("robotLegR1", robotLegR1);
            sprites.Add("robotLegR2", robotLegR2);
            sprites.Add("robotLegR3", robotLegR3);
            #endregion

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
