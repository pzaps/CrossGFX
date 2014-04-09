using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace crossGFX.XNARenderer
{
    public class Window : Game, IWindow
    {
        GUI.GUI gui;
        RenderTarget renderTarget;
        InputHelper inputHelper;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public event EventHandler<TickEventArgs> OnTick;

        public event EventHandler OnInitialize;

        public event EventHandler<TickEventArgs> OnDraw;

        public SpriteBatch SpriteBatch {
            get { return spriteBatch; }
        }

        public GraphicsDeviceManager GraphicsDeviceManager {
            get { return graphics; }
        }

        protected override void Update(GameTime gameTime) {
#if WINDOWS_PHONE
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
#endif

            inputHelper.UpdateInput();

            if (OnTick != null) {
                OnTick(this, new TickEventArgs(DriverManager.ActiveDriver.System.GetTickCount()));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            if (gameTime.IsRunningSlowly) {
                GraphicsDevice.Clear(Color.Red);
            } else {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }

            this.spriteBatch.Begin();
            if (OnDraw != null) {
                OnDraw(this, new TickEventArgs(DriverManager.ActiveDriver.System.GetTickCount()));
            }
            if (GUI.Initialized) {
                GUI.RenderCanvas();
            }
            this.spriteBatch.End();

            base.Draw(gameTime);
        }

#if WINDOWS
        public Window(int width, int height, bool fullScreen) {
#elif WINDOWS_PHONE
        public Window() {
#endif
            gui = new GUI.GUI(DriverManager.ActiveDriver, this);

            graphics = new GraphicsDeviceManager(this);
            inputHelper = new XNARenderer.InputHelper(this);

#if WINDOWS
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.IsFullScreen = fullScreen;
            graphics.ApplyChanges();
#elif WINDOWS_PHONE
            DriverManager.AssignDriverInstance(new Driver());
            DriverManager.ActiveDriver.Initialize();

            System sys = DriverManager.ActiveDriver.System as System;
            sys.GameWindow = this;
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            Window.OrientationChanged += new EventHandler<EventArgs>(Window_OrientationChanged);
#endif

            Content.RootDirectory = "Content";
        }

        void Window_OrientationChanged(object sender, EventArgs e) {
            RaiseOnResized();
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.renderTarget = new RenderTarget(GraphicsDevice, spriteBatch);

            if (OnInitialize != null) {
                OnInitialize(this, EventArgs.Empty);
            }
        }

        public Input.IInputHelper InputHelper {
            get { return inputHelper; }
        }

        public string Title {
            set { base.Window.Title = value; }
        }

        public GUI.GUI GUI {
            get { return gui; }
        }

        public void Think() {
            // TODO: XNA: Think()
        }

        public int Width {
            get { return GraphicsDevice.Viewport.Width; }
        }

        public int Height {
            get { return GraphicsDevice.Viewport.Height; }
        }

        #region RenderTarget implementation

        public void Fill(global::System.Drawing.Color color) {
            renderTarget.Fill(color);
        }

        public void Fill(global::System.Drawing.Rectangle bounds, global::System.Drawing.Color color) {
            renderTarget.Fill(bounds, color);
        }

        public void DrawLine(int x, int y, int a, int b, global::System.Drawing.Color color) {
            renderTarget.DrawLine(x, y, a, b, color);
        }

        public void Draw(ITexture texture, global::System.Drawing.Point position) {
            renderTarget.Draw(texture, position);
        }

        public void Draw(ITexture texture, global::System.Drawing.Point position, global::System.Drawing.Rectangle sourceRectangle) {
            renderTarget.Draw(texture, position, sourceRectangle);
        }

        public void DrawStretched(ITexture texture, global::System.Drawing.Rectangle destinationBounds, global::System.Drawing.Rectangle sourceRectangle) {
            renderTarget.DrawStretched(texture, destinationBounds, sourceRectangle);
        }

        public void StartClip(global::System.Drawing.Rectangle clipRegion, int scale) {
            renderTarget.StartClip(clipRegion, scale);
        }

        public void EndClip() {
            renderTarget.EndClip();
        }

        public void BeginMoveOrigin(global::System.Drawing.Point newOrigin) {
            throw new NotImplementedException();
        }

        public void EndMoveOrigin() {
            throw new NotImplementedException();
        }

        public void DrawString(IFont font, string text, int textSize, global::System.Drawing.Color textColor, global::System.Drawing.Point position) {
            font.RenderText(text, textSize, textColor, this, position);
        }

        public void DrawStretched(ITexture texture, global::System.Drawing.Rectangle destinationBounds) {
            DrawStretched(texture, destinationBounds, new global::System.Drawing.Rectangle(0, 0, texture.Width, texture.Height));
        }

        #endregion

        internal void RaiseOnResized() {
            if (Resized != null) {
                Resized(this, EventArgs.Empty);
            }
        }

        public event EventHandler Resized;

        public void BeginDrawing() {
        }

        public void EndDrawing() {
        }

        public void Close() {
            base.Exit();
        }

#if WINDOWS
        public global::System.Drawing.Icon Icon {
            set {
                // Do nothing... unsupported...
            }
        }
#endif


       
    }
}
