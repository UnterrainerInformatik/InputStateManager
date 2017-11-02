using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GamePad = OpenTK.Input.GamePad;
using Keyboard = OpenTK.Input.Keyboard;
using Mouse = InputStateManager.Mouse;

namespace Test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public const int MIN_SCREEN_RESOLUTION_WIDTH = 1024;
        public const int MIN_SCREEN_RESOLUTION_HEIGHT = 768;

        SpriteBatch spriteBatch;
        private SpriteFont font;
        private readonly InputStateManager.InputStateManager input;

        public Game1()
        {
            var graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = MIN_SCREEN_RESOLUTION_WIDTH;
            graphics.PreferredBackBufferHeight = MIN_SCREEN_RESOLUTION_HEIGHT;
            graphics.IsFullScreen = false;
            graphics.PreparingDeviceSettings += PrepareDeviceSettings;
            graphics.SynchronizeWithVerticalRetrace = true;

            IsMouseVisible = true;
            IsFixedTimeStep = true;

            Content.RootDirectory = "Content";
            input = new InputStateManager.InputStateManager();
        }

        void PrepareDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.HiDef;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("AnonymousPro8");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (input.GamePad.IsPress(Buttons.Back) || input.Keyboard.IsPress(Keys.Escape))
                Exit();
            input.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            DrawText();

            base.Draw(gameTime);
        }

        private void DrawText()
        {
            StringBuilder b = new StringBuilder();
            b.Append($"Mouse Button Left Down: {input.Mouse.IsDown(Mouse.Button.LEFT)}\n");
            b.Append($"Mouse Button Middle Down: {input.Mouse.IsDown(Mouse.Button.MIDDLE)}\n");
            b.Append($"Mouse Button Right Down: {input.Mouse.IsDown(Mouse.Button.RIGHT)}\n");

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.DrawString(font, b, new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
    }
}
