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
        #region Fields

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static List<GameObject> activeGameObjects = new List<GameObject>();
        private static List<GameObject> gameObjectsToBeAdded = new List<GameObject>();
        private static List<GameObject> gameObjectsToBeRemoved = new List<GameObject>();
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
        public static SpriteFont textFont;
        private static Gameworld activeGameWorld;
        public static Vector2 startingPosition = new Vector2(1400, -100);
        private static Texture2D backgroundTexture;

        public static bool orderOnGoing;
        private Song backgroundMusic;
        public static string order;
        public static int orderNumber=1;
        public static int point;

        internal static SnapBoard snapBoard;
        internal static ConveyorBelt conveyorBelt;

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

        /// <summary>
        /// Property to get/set Grabbing bool (prevents more than one object from being drag n' dropped simultaniuously)
        /// </summary>
        public static bool Grabbing { get => grabbing; set => grabbing = value; }

        #endregion

        #region Constructors
        public Gameworld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            activeGameWorld = this;
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
            textFont = Content.Load<SpriteFont>("spriteFont");

            //Creation of MousePointer, MUST BE AFTER loading of sprites
            mousePointer = new MousePointer();
            snapBoard = new SnapBoard();
            activeGameObjects.Add(snapBoard);
            AddGameObject(new Timer(60));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            AddGameObject(new Head(1));
            conveyorBelt = new ConveyorBelt(new Vector2(1550, 100));
            AddGameObject(conveyorBelt);
            AddGameObject(new StartOrderAndEndResultsBoard(1));
            Button finishBuildBtn = new Button(true);
            finishBuildBtn.Position = new Vector2(1050, 200);
            AddGameObject(finishBuildBtn);
            AddGameObject(new OrderPaper("test message", new Vector2(1050, 1300)));
            backgroundTexture = Content.Load<Texture2D>("Sprites\\Background\\background_pattern");

            gameObjectsToBeAdded.Add(new Head(1));
            gameObjectsToBeAdded.Add(new Torso(1));
            gameObjectsToBeAdded.Add(new Arm(4));
            gameObjectsToBeAdded.Add(new Arm(1));
            gameObjectsToBeAdded.Add(new Leg(4));
            gameObjectsToBeAdded.Add(new Leg(1));
            gameObjectsToBeAdded.Add(new TrickPart(1));
            backgroundMusic = music["backgroundMusic1"];
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
            Timer.Stop();
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
            mousePointer.Update();

            foreach (GameObject gameObject in activeGameObjects)
            {
                if (!grabbing)
                    mousePointer.CheckCollision(gameObject);
                if (snapBoard.timer == null)
                    if (gameObject is Timer)
                        snapBoard.timer = gameObject as Timer;
                if (gameObject.RemoveThis)
                    RemoveGameObject(gameObject);
                gameObject.Update(gameTime);
            }
            foreach (GameObject gameObject in gameObjectsToBeRemoved)
            {
                activeGameObjects.Remove(gameObject);
            }
            gameObjectsToBeRemoved.Clear();
            activeGameObjects.AddRange(gameObjectsToBeAdded);
            gameObjectsToBeAdded.Clear();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSteelBlue);
#if DEBUG
            bool disableCollisionDrawing = Keyboard.GetState().IsKeyDown(Keys.Space);
#endif
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
            _spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

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
            if (disableCollisionDrawing)
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

            Texture2D timerBackground = Content.Load<Texture2D>("Sprites\\Timer\\basic timer background");
            sprites.Add("timerBackground", timerBackground);

            Texture2D timerForeground = Content.Load<Texture2D>("Sprites\\Timer\\basic timer foreground");
            sprites.Add("timerForeground", timerForeground);

            Texture2D button = Content.Load<Texture2D>("Sprites\\simpleButton2");
            sprites.Add ("button", button);

            Texture2D conveyor = Content.Load<Texture2D>("Sprites\\basic conveyor");
            sprites.Add("conveyorBelt", conveyor);

            Texture2D orderPaper= Content.Load<Texture2D>("Sprites\\order paper");
            sprites.Add("orderPaper", orderPaper);

            Texture2D orderBoard= Content.Load<Texture2D>("Sprites\\orderboard");
            sprites.Add("orderBoard", orderBoard);

            Texture2D resultBoard1= Content.Load<Texture2D>("Sprites\\results\\resultBoard1");
            sprites.Add("resultBoard1",resultBoard1);

            Texture2D resultBoard2 = Content.Load<Texture2D>("Sprites\\results\\resultBoard3");
            sprites.Add("resultBoard2", resultBoard2);

            Texture2D smiley1= Content.Load<Texture2D>("Sprites\\results\\goodSmiley");
            sprites.Add("goodSmiley", smiley1);

            Texture2D smiley2 = Content.Load<Texture2D>("Sprites\\results\\badSmiley");
            sprites.Add("badSmiley", smiley2);

            Texture2D smiley3 = Content.Load<Texture2D>("Sprites\\results\\midSmiley");
            sprites.Add("midSmiley", smiley3);

            #region parts
            Texture2D robotHead1 = Content.Load<Texture2D>("Sprites\\Robotparts\\head1");
            Texture2D robotHead2 = Content.Load<Texture2D>("Sprites\\Robotparts\\head2");
            Texture2D robotHead3 = Content.Load<Texture2D>("Sprites\\Robotparts\\head3");

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
            #region trickParts
            Texture2D trickPart1 = Content.Load<Texture2D>("Sprites\\trickParts\\trickPart1");
            Texture2D trickPart2 = Content.Load<Texture2D>("Sprites\\trickParts\\trickPart2");
            Texture2D trickPart3 = Content.Load<Texture2D>("Sprites\\trickParts\\trickPart3");

            sprites.Add("trickPart1", trickPart1);
            sprites.Add("trickPart2", trickPart2);
            sprites.Add("trickPart3", trickPart3);

            //new trickparts
            Texture2D trickPartHead = Content.Load<Texture2D>("Sprites\\trickParts\\trickPartHead");
            Texture2D trickPartBody = Content.Load<Texture2D>("Sprites\\trickParts\\trickPartBody");
            Texture2D trickPartArm1 = Content.Load<Texture2D>("Sprites\\trickParts\\trickPartArm1");
            Texture2D trickPartArm2 = Content.Load<Texture2D>("Sprites\\trickParts\\trickPartArm2");
            Texture2D trickPartLeg = Content.Load<Texture2D>("Sprites\\trickParts\\trickPartLeg");

            sprites.Add("trickPartHead", trickPartHead);
            sprites.Add("trickPartBody", trickPartBody);
            sprites.Add("trickPartArm1", trickPartArm1);
            sprites.Add("trickPartArm2", trickPartArm2);
            sprites.Add("trickPartLeg", trickPartLeg);
            #endregion
            #region trash and square
            Texture2D trashcan = Content.Load<Texture2D>("Sprites\\trash");
            sprites.Add("trashcan", trashcan);

            Texture2D square = Content.Load<Texture2D>("Sprites\\square");
            sprites.Add("square", square);
            #endregion
            #region Snapboard

            Texture2D snapBoard = Content.Load<Texture2D>("Sprites\\SnapBoard\\blankSlot");
            sprites.Add("snapBoard", snapBoard);

            #endregion
            #region cutscenes
            Texture2D customerEnter1 = Content.Load<Texture2D>("Sprites\\cutscenes\\cutscenes1");
            sprites.Add("enterCutscene1", customerEnter1);
            Texture2D customerEnter2 = Content.Load<Texture2D>("Sprites\\cutscenes\\cutscenes2");
            sprites.Add("enterCutscene2", customerEnter2);
            Texture2D customerEnter3 = Content.Load<Texture2D>("Sprites\\cutscenes\\cutscenes3");
            sprites.Add("enterCutscene3", customerEnter3);
            Texture2D customerEnter4 = Content.Load<Texture2D>("Sprites\\cutscenes\\cutscenes4");
            sprites.Add("enterCutscene4", customerEnter4);
            Texture2D customerEnter5 = Content.Load<Texture2D>("Sprites\\cutscenes\\cutscenes5");
            sprites.Add("orderCutscene", customerEnter5);

            Texture2D win = Content.Load<Texture2D>("Sprites\\cutscenes\\cutscenes_win");
            sprites.Add("winScreen", win);

            Texture2D lose = Content.Load<Texture2D>("Sprites\\cutscenes\\cutscenes_lose");
            sprites.Add("loseScreen", lose);
            #endregion
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

            #region Movement

            SoundEffect moveSound = Content.Load<SoundEffect>("Lyd\\Movement\\moveSound");
            SoundEffect rotateSound = Content.Load<SoundEffect>("Lyd\\Movement\\rotateSound");

            sounds.Add("moveSound", moveSound);
            sounds.Add("rotateSound", rotateSound);

            #endregion
            #region SnapBoard

            SoundEffect buildSound = Content.Load<SoundEffect>("Lyd\\SnapBoard\\buildSound");
            SoundEffect trashSound = Content.Load<SoundEffect>("Lyd\\SnapBoard\\trashSound");

            sounds.Add("buildSound", buildSound);
            sounds.Add("trashSound", trashSound);

            #endregion
            #region Review

            SoundEffect badReview = Content.Load<SoundEffect>("Lyd\\Review\\badReviewSound");
            SoundEffect averageReview = Content.Load<SoundEffect>("Lyd\\Review\\averageReviewSound");
            SoundEffect goodReview = Content.Load<SoundEffect>("Lyd\\Review\\goodReviewSound");

            sounds.Add("badReview", badReview);
            sounds.Add("averageReview", averageReview);
            sounds.Add("goodReview", goodReview);

            #endregion
            #region Misc

            SoundEffect ding = Content.Load<SoundEffect>("Lyd\\Misc\\dingSound");

            sounds.Add("ding", ding);

            #endregion

        }

        /// <summary>
        /// Method to supply "music" dictionary with content
        /// </summary>
        /// <param name="music">Dictionary to have "Songs" added to</param>
        private void AddMusic(Dictionary<string, Song> music)
        {
            Song music1 = Content.Load<Song>("Music\\steampunk-garage-142325");
            music.Add("backgroundMusic1",music1);
        }

        #endregion

        /// <summary>
        /// Gameobject will be added after next Update
        /// </summary>
        /// <param name="gameObject"></param>
        public static void AddGameObject(GameObject gameObject)
        {
            gameObjectsToBeAdded.Add(gameObject);
            gameObject.LoadContent(activeGameWorld.Content);
        }

        /// <summary>
        /// Gameobject will be removed after next Update
        /// </summary>
        /// <param name="gameObject"></param>
        public static void RemoveGameObject(GameObject gameObject)
        {
            gameObjectsToBeRemoved.Add(gameObject);
        }


#if DEBUG

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
#endif
               
        #endregion
    }
}
