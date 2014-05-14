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
    enum ActiveCamera
    {
        Follow = 1,
        Free,
        Aerial
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        float movementSpeed = 1f;
        const float DefaultMouseSensitivity = 0.005f;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle FullScreen, PIP1, PIP2;
        RenderTarget2D RT_PIP1, RT_PIP2;
        bool showPIP = true;
        ActiveCamera activeCamera = ActiveCamera.Free;
        public Matrix ProjectionMatrix;
        Camera FreeMove, Follow, MiniMap;
        private bool pause = false;

        public Revolving_ball ball;
        Vector3 lastPosition;

        const int mouseCenterX = 100, mouseCenterY = 100;
        MouseState preCaptureMouse;
        bool captureMouse = false;

        public bool CaptureMouse
        {
            get { return captureMouse; }
            set { 
                if (captureMouse != value) {
                    if (captureMouse = value)
                    {
                        preCaptureMouse = curMouse;
                        Mouse.SetPosition(mouseCenterX, mouseCenterY);
                        IsMouseVisible = false;
                    }
                    else
                    {
                        if (preCaptureMouse != null)
                        {
                            Mouse.SetPosition(preCaptureMouse.X, preCaptureMouse.Y);
                        }
                        IsMouseVisible = true;
                    }
                }
            }
        }
        MouseState lastMouse, curMouse;
        KeyboardState lastKey;

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
            IsMouseVisible = true;

            //create 3 Rectangles
            FullScreen = GraphicsDevice.Viewport.Bounds;
            float scaleFactor = 2.5f;
            PIP1 = new Rectangle(FullScreen.Width - (int)(FullScreen.Width / scaleFactor), 0, (int)(FullScreen.Width / scaleFactor), (int)(FullScreen.Height / scaleFactor));
            PIP2 = new Rectangle(FullScreen.Width - (int)(FullScreen.Width / scaleFactor), FullScreen.Height - (int)(FullScreen.Height / scaleFactor),
                (int)(FullScreen.Width / scaleFactor), (int)(FullScreen.Height / scaleFactor));

            //create 2 render targets
            RT_PIP1 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            RT_PIP2 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);

            //set up projection matrix
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), GraphicsDevice.Viewport.AspectRatio, 0.01f, 100.0f);

            //create cameras
            FreeMove = new Camera(this, new Vector3(2, 0, 0));
            FreeMove.UpAxis = Vector3.UnitZ;
            FreeMove.ForwardAxis = -Vector3.UnitX;
            FreeMove.MinPitch = MathHelper.ToRadians(-89.9f);
            FreeMove.MaxPitch = MathHelper.ToRadians(89.9f);


            Follow = new Camera(this, new Vector3(2, 2, 0));
            Follow.UpAxis = Vector3.UnitZ;
            Follow.ForwardAxis = -Vector3.UnitX;
            Follow.MinPitch = MathHelper.ToRadians(-89.9f);
            Follow.MaxPitch = MathHelper.ToRadians(89.9f);

            MiniMap = new Camera(this, new Vector3(0, 0, 3f), MathHelper.ToRadians(90f), -MathHelper.ToRadians(90f));
            MiniMap.UpAxis = Vector3.UnitZ;
            MiniMap.ForwardAxis = -Vector3.UnitX;
            MiniMap.MinPitch = MathHelper.ToRadians(-89.9f);
            MiniMap.MaxPitch = MathHelper.ToRadians(89.9f);

            //load model
            ball = new Revolving_ball(this);

            //TODO load elements!
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
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector3 moveVector = Vector3.Zero;

            KeyboardState keyState = Keyboard.GetState();
            if (lastKey == null)
            {
                lastKey = keyState;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyState.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: complete keyboard logic here
            foreach (Keys k in keyState.GetPressedKeys()) {
                switch (k)
                {
                    case Keys.D1:
                    case Keys.D2:
                    case Keys.D3:
                        activeCamera = (ActiveCamera)(k - Keys.D0);
                        break;
                    case Keys.P:
                        if (lastKey.IsKeyUp(Keys.P))
                        {
                            showPIP = !showPIP; //only togle on first key press
                        }
                        break;
                    case Keys.W: //free camera up
                    case Keys.Up:
                        moveVector.X -= 1f;
                        break;
                    case Keys.S: //free camera down
                    case Keys.Down:
                        moveVector.X = 1f;
                        break;
                    case Keys.A: //free camera left
                    case Keys.Left:
                        moveVector.Y -= 1f;
                        break;
                    case Keys.D: //free camera right
                    case Keys.Right:
                        moveVector.Y = 1f;
                        break;
                    case Keys.N: //step scene forward
                        if (pause)
                        {
                            if (lastKey.IsKeyUp(Keys.N))
                            {

                            }
                        }
                        break;
                    case Keys.Enter: //Reset
                        if (lastKey.IsKeyUp(Keys.Enter))
                        {

                        }
                        break;
                    case Keys.Space: //Start/Stop
                        if (lastKey.IsKeyUp(Keys.Space))
                        {
                            pause = !pause;
                            //TODO: also set pause state of physics engine
                        }
                        break;
                }
            }

            // TODO: compile mouse logic here
            curMouse = Mouse.GetState();
            if (lastMouse == null)
            {   
                lastMouse = curMouse;
            }

            Vector2 mouseDelta = new Vector2(
                    captureMouse ? curMouse.X - mouseCenterX : 0,
                    captureMouse ? curMouse.Y - mouseCenterY : 0
                );

            if (activeCamera == ActiveCamera.Free || showPIP)
            {
                this.CaptureMouse = curMouse.RightButton == ButtonState.Pressed;

                FreeMove.Pitch -= mouseDelta.Y * DefaultMouseSensitivity;
                FreeMove.Yaw -= mouseDelta.X * DefaultMouseSensitivity;

                //freemove camera logic
                if (moveVector != Vector3.Zero)
                {
                    moveVector.Normalize();
                    moveVector *= movementSpeed * delta;
                    FreeMove.Move(moveVector);
                }
            }
            
            if (captureMouse)
            {
                Mouse.SetPosition(mouseCenterX, mouseCenterY);
            }


            if (!pause)
            {
                //update animated objects
                ball.update(gameTime);
                if (lastPosition == null)
                {
                    lastPosition = ball.location;
                }
                //TODO: add follow camera update logic here
                Vector3 direction = ball.location - lastPosition;
                direction.Normalize();
                Follow.ForwardAxis = direction;
                Follow.Position = direction;
                Follow.Move(-direction * 5f);
                Follow.Position += 2*Follow.UpAxis + Follow.SideAxis;
                Follow.Yaw = MathHelper.ToRadians(00f);
                Follow.Pitch = MathHelper.ToRadians(50f);

                //remember new position
                lastPosition = ball.location;
            }

            

            //TODO consider adding statistic update code here like FPS

            //store last key state
            lastKey = keyState;
            lastMouse = curMouse;


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Draw PIP1
            GraphicsDevice.SetRenderTarget(RT_PIP1);
            if (activeCamera != ActiveCamera.Follow)
            {
                drawFollowCamera();
            }
            else
            {
                drawFreeCamera();
            }

            //Draw PIP2
            GraphicsDevice.SetRenderTarget(RT_PIP2);
            if (activeCamera != ActiveCamera.Aerial)
            {
                drawAerialCamera();
            }
            else
            {
                drawFreeCamera();
            }

            //Draw full screen
            GraphicsDevice.SetRenderTarget(null);
            switch (activeCamera)
            {
                case ActiveCamera.Aerial:
                    drawAerialCamera();
                    break;
                case ActiveCamera.Follow:
                    drawFollowCamera();
                    break;
                case ActiveCamera.Free:
                    drawFreeCamera();
                    break;
            }

            //Draw PIP to full screen
            spriteBatch.Begin();
            if (showPIP)
            {
                spriteBatch.Draw(RT_PIP1, PIP1, Color.White);
                spriteBatch.Draw(RT_PIP2, PIP2, Color.White);
            }
            //TODO add HUD style code here
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawFreeCamera()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //TODO add free camera drawing code here
            FreeMove.draw();
        }

        private void drawFollowCamera()
        {
            GraphicsDevice.Clear(Color.Black);

            //TODO add follow camera drawing here
            Follow.draw();
        }

        private void drawAerialCamera()
        {
            GraphicsDevice.Clear(Color.White);

            //TODO add aerial camera drawing here.
            MiniMap.draw();
        }
    }
}
