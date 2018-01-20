// *************************************************************************** 
// This is free and unencumbered software released into the public domain.
// 
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.
// 
// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>
// ***************************************************************************

using System.Text;
using InputStateManager;
using InputStateManager.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mouse = InputStateManager.Inputs.Mouse;

namespace Test
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public const int MIN_SCREEN_RESOLUTION_WIDTH = 1024;
        public const int MIN_SCREEN_RESOLUTION_HEIGHT = 768;

        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private readonly InputManager input = new InputManager();

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
        }

        private void PrepareDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.HiDef;
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("AnonymousPro8");
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (input.Pad.Is.Press(Buttons.Back) || input.Key.Is.Press(Keys.Escape))
                Exit();
            input.Update();
            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (input.Mouse.Is.Press(Mouse.Button.LEFT))
                GraphicsDevice.Clear(Color.CornflowerBlue);
            else if (input.Mouse.Is.Release(Mouse.Button.LEFT))
                GraphicsDevice.Clear(Color.Coral);
            else GraphicsDevice.Clear(Color.Black);
            DrawText();
            base.Draw(gameTime);
        }

        private void DrawText()
        {
            StringBuilder b = new StringBuilder();
            b.Append($"Mouse Button Left Down: {input.Mouse.Is.Down(Mouse.Button.LEFT)} " +
                     $"Pressed: {input.Mouse.Is.Press(Mouse.Button.LEFT)} " +
                     $"Released: {input.Mouse.Is.Release(Mouse.Button.LEFT)}\n");
            b.Append($"Mouse Button Middle Down: {input.Mouse.Is.Down(Mouse.Button.MIDDLE)} " +
                     $"Pressed: {input.Mouse.Is.Press(Mouse.Button.MIDDLE)} " +
                     $"Released: {input.Mouse.Is.Release(Mouse.Button.MIDDLE)}\n");
            b.Append($"Mouse Button Right Down: {input.Mouse.Is.Down(Mouse.Button.RIGHT)} " +
                     $"Pressed: {input.Mouse.Is.Press(Mouse.Button.RIGHT)} " +
                     $"Released: {input.Mouse.Is.Release(Mouse.Button.RIGHT)}\n");
            b.Append($"GamePad: {input.Pad.Is.DPad.Down(Pad.DPadDirection.UP)}\n");
            input.Pad.Is.Triggers.Right();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.DrawString(font, b, new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
    }
}