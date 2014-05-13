using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Russell_Peake_Project
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle FullScreen, PIP1, PIP2;
        RenderTarget2D RT_PIP1, RT_PIP2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;

            Content.RootDirectory = "Content";
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

            //create 3 Rectangles
            FullScreen = GraphicsDevice.Viewport.Bounds;
            float scaleFactor = 2.5f;
            PIP1 = new Rectangle(FullScreen.Width - (int)(FullScreen.Width / scaleFactor), 0, (int)(FullScreen.Width / scaleFactor), (int)(FullScreen.Height / scaleFactor));
            PIP2 = new Rectangle(FullScreen.Width - (int)(FullScreen.Width / scaleFactor), FullScreen.Height - (int)(FullScreen.Height / scaleFactor),
                (int)(FullScreen.Width / scaleFactor), (int)(FullScreen.Height / scaleFactor));

            //create 2 render targets
            RT_PIP1 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            RT_PIP2 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            KeyboardState keyState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyState.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //set up 3 methods which draws the three camera modes and use a variable to change between the modes
            //after setting render target (instead of clears)

            //Draw PIP1
            GraphicsDevice.SetRenderTarget(RT_PIP1);
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add PIP1 drawing handling here

            //Draw PIP2
            GraphicsDevice.SetRenderTarget(RT_PIP2);
            GraphicsDevice.Clear(Color.White);

            // TODO Add PIP2 drawing handling here

            //Draw full screen
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            //Draw PIP to full screen
            spriteBatch.Begin();
            spriteBatch.Draw(RT_PIP1, PIP1, Color.White);
            spriteBatch.Draw(RT_PIP2, PIP2, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
