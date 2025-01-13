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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
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

        public Gameworld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

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

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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

            // TODO: Add your drawing code here
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            mousePointer.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }


        private void AddSprites(Dictionary<string, Texture2D> sprites)
        {
            Texture2D mouse = Content.Load<Texture2D>("Sprites\\Mouse\\screwdriver_mousepointer");

            sprites.Add("mouse", mouse);
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
    }
}
