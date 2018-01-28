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

using System;
using System.Linq;
using System.Text;
using InputStateManager;
using InputStateManager.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Mouse = InputStateManager.Inputs.Mouse;

namespace TestAndroid
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private SpriteBatch spriteBatch;

        private readonly InputManager input = new InputManager();
        private SpriteFont font;
        private Texture2D point;

        public Game1()
        {
            var graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
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
            point = Content.Load<Texture2D>("point");
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (input.Pad().Is.Press(Buttons.Back))
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (input.Mouse.Is.Press(Mouse.Button.LEFT))
                GraphicsDevice.Clear(Color.CornflowerBlue);
            else if (input.Mouse.Is.Release(Mouse.Button.LEFT))
                GraphicsDevice.Clear(Color.Coral);
            else
                GraphicsDevice.Clear(Color.Black);
            DrawText();
            DrawTouches();
            base.Draw(gameTime);
        }

        private void DrawTouches()
        {
            Vector2 o = point.Bounds.Size.ToVector2() / 2f;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            foreach (var l in input.Touch.Is.Collection)
            {
                if (l.State == TouchLocationState.Pressed || l.State == TouchLocationState.Moved)
                {
                    spriteBatch.Draw(point, l.Position, null, Color.White, 0, o, 4 * Vector2.One,
                        SpriteEffects.None, 1);
                }
            }

            spriteBatch.End();
        }

        private string GetEnabledGestures(string prefix)
        {
            var r = "";
            foreach (var n in Enum.GetNames(typeof(GestureType)))
            {
                if (Enum.TryParse(n, out GestureType t) && ((int)input.Touch.EnabledGestures & (int) t) != 0)
                    r += prefix + n + "\n";
            }
            return r;
        }

        private void DrawText()
        {
            var b = new StringBuilder();
            b.Append("This is a test-application. Use the back-button to exit like you would normally do.\n");
            b.Append("You can rotate your phone. Play around and it will display debug-data.\n");
            b.Append("--- Data --------------------------------------------------------------------------\n");
            b.Append($"Display: {input.Touch.DisplayWidth}x{input.Touch.DisplayHeight} orientation:{input.Touch.DisplayOrientation}\n");
            b.Append($"Info: whandle:{input.Touch.WindowHandle} gestures:{input.Touch.EnabledGestures}\n");
            b.Append(GetEnabledGestures("  "));
            b.Append("--- TouchLocations ----------------------------------------------------------------\n");
            b.Append($"Number of TouchLocations: {input.Touch.Is.Collection.Count}\n");
            var i = 0;
            foreach (var t in input.Touch.Is.Collection)
            {
                b.Append($"  {++i}: {t.Id}-p:{t.Pressure} pos:{t.Position} state:{t.State}\n");
            }
            
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.DrawString(font, b, new Vector2(10, 10), Color.White, 0, Vector2.Zero, 4 * Vector2.One,
                SpriteEffects.None, 1);
            spriteBatch.End();
        }
    }
}