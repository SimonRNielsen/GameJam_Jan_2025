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
        #region Fields

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //lists that are important
        private List<GameObject> activeGameObjects = new List<GameObject>();
        private List<GameObject> gameObjectsToBeAdded = new List<GameObject>();
        private List<GameObject> gameObjectsToBeRemoved = new List<GameObject>();
        private Vector2 screenSize;
        private static Vector2 mousePosition;
        private static bool mouseLeftClick;
        private static bool mouseRightClick;
        private static MousePointer mousePointer;
        private static bool grabbing = false;
        public static Dictionary<string, Texture2D> sprites = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D[]> animations = new Dictionary<string, Texture2D[]>();
        public static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        public static Dictionary<string, Song> music = new Dictionary<string, Song>();

        internal static SnapBoard snapBoard;

        #endregion

        #region Properties

        /// <summary>
        /// Property to register left click on mouse
        /// </summary>
        public static bool MouseLeftClick { get => mouseLeftClick; }

        /// <summary>
        /// Property to register right click on mouse
        /// </summary>
        public static bool MouseRightClick { get => mouseRightClick; }

        /// <summary>
        /// Property to register the position of the mouse
        /// </summary>
        public static Vector2 MousePosition { get => mousePosition; }


        public static bool Grabbing { get => grabbing; set => grabbing = value; }

        #endregion

        #region Constructors
        public Gameworld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        #endregion

        #region Methods
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.ApplyChanges();
            screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            //Adds assets
            AddSprites(sprites);
            AddAnimation(animations);
            AddSounds(sounds);
            AddMusic(music);

            //Creation of MousePointer, MUST BE AFTER loading of sprites
            mousePointer = new MousePointer();
            snapBoard = new SnapBoard();
            activeGameObjects.Add(new TestDummy(new Vector2(1500, 500)));
            activeGameObjects.Add(new TestDummy(new Vector2(1000, 500)));

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

            // Handling and updating mouse input
            var mouseState = Mouse.GetState();
            mousePosition = new Vector2(mouseState.Position.X, mouseState.Position.Y);
            mouseLeftClick = mouseState.LeftButton == ButtonState.Pressed;
            mouseRightClick = mouseState.RightButton == ButtonState.Pressed;
            mousePointer.Update(gameTime);

            foreach (GameObject gameObject in activeGameObjects)
            {
                if (!grabbing)
                    mousePointer.CheckCollision(gameObject);
                gameObject.Update(gameTime, screenSize);
                if (gameObject.RemoveThis)
                    RemoveGameObject(gameObject);
            }
            foreach (GameObject gameObject in gameObjectsToBeRemoved)
            {
                activeGameObjects.Remove(gameObject);
            }
            gameObjectsToBeRemoved.Clear();
            foreach (GameObject gameObject in gameObjectsToBeAdded)
            {
                activeGameObjects.Add(gameObject);
            }
            gameObjectsToBeAdded.Clear();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
#if DEBUG
            bool disableCollisionDrawing = Keyboard.GetState().IsKeyDown(Keys.Space);
#endif
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            foreach (GameObject gameObject in activeGameObjects)
            {
                gameObject.Draw(_spriteBatch);
#if DEBUG
                if (disableCollisionDrawing)
                {
                    DrawCollisionBox(gameObject);
                }
#endif
            }

#if DEBUG
            foreach (Rectangle rectangle in snapBoard.parts.Keys)
            {
                DrawDragNDropBoxes(rectangle);
            }
#endif
            mousePointer.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Assets

        /// <summary>
        /// Method to supply "sprites" dictionary with content
        /// </summary>
        /// <param name="sprites">Dictionary to have sprites added to</param>
        private void AddSprites(Dictionary<string, Texture2D> sprites)
        {

#if DEBUG

            Texture2D debug = Content.Load<Texture2D>("Sprites\\DEBUG\\pixel");
            Texture2D testdummy = Content.Load<Texture2D>("Sprites\\DEBUG\\testdummy");

            sprites.Add("debug", debug);
            sprites.Add("testdummy", testdummy);

#endif

            Texture2D mouse = Content.Load<Texture2D>("Sprites\\Mouse\\screwdriver_mousepointer");

            sprites.Add("mouse", mouse);

        }

        /// <summary>
        /// Method to supply "animations" dictionary with content
        /// </summary>
        /// <param name="animations">Dictionary to have animation arrays added to</param>
        private void AddAnimation(Dictionary<string, Texture2D[]> animations)
        {

        }

        /// <summary>
        /// Method to supply "sounds" dictionary with content
        /// </summary>
        /// <param name="sounds">Dictionary to have soundeffects added to</param>
        private void AddSounds(Dictionary<string, SoundEffect> sounds)
        {

        }

        /// <summary>
        /// Method to supply "music" dictionary with content
        /// </summary>
        /// <param name="music">Dictionary to have "Songs" added to</param>
        private void AddMusic(Dictionary<string, Song> music)
        {

        }

        #endregion

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


        private void DrawCollisionBox(GameObject gameObject)
        {
            Color color = Color.Red;
            Rectangle collisionBox = gameObject.CollisionBox;
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

            _spriteBatch.Draw(sprites["debug"], topLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.Draw(sprites["debug"], bottomLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.Draw(sprites["debug"], rightLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.Draw(sprites["debug"], leftLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
        }

        private void DrawDragNDropBoxes(Rectangle rectangle)
        {
            Color color = Color.Red;
            Rectangle collisionBox = rectangle;
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

            _spriteBatch.Draw(sprites["debug"], topLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.Draw(sprites["debug"], bottomLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.Draw(sprites["debug"], rightLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.Draw(sprites["debug"], leftLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
        }
        #endregion
    }
}
