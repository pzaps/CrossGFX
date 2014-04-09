// Copyright (c) 2014 CrossGFX Team

// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation;
// version 3.0.

// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, visit
// https://www.gnu.org/licenses/lgpl.html.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using crossGFX;
using crossGFX.GUI;
using crossGFX.GUI.Skins;

namespace TestApplication
{
    class Program
    {
        //static int clickCount = 0;

        //static Point[] shipPositions;
        static Point shipPosition = new Point(100, 100);

        static ITexture texture;
        static IFont font;
        //static bool shouldIncreaseAlpha = false;

        public static string basePath;

        //static DebugWindow debugWindow;

        static Scene scene;

        static void Main(string[] args) {
            SystemLoader.Load(ref basePath);
            IWindow window = DriverManager.ActiveDriver.System.CreateWindow(800, 600, false);

            DriverManager.ActiveDriver.System.TargetFPS = 60;

            window.Draw += new EventHandler<TickEventArgs>(window_OnDraw);
            window.Update += new EventHandler<TickEventArgs>(window_OnTick);
            window.OnInitialize += new EventHandler(window_OnInitialize);

            DriverManager.ActiveDriver.System.Run();
            DriverManager.ActiveDriver.System.Cleanup();
        }
   
        static void window_OnTick(object sender, TickEventArgs e) {
            IWindow window = sender as IWindow;

            scene.Tick(window, e);
        }

        static void window_OnInitialize(object sender, EventArgs e) {
            IWindow window = sender as IWindow;

            texture = DriverManager.ActiveDriver.ResourceManager.LoadTexture(basePath + "resources/sprites/testsprite.png");
            font = DriverManager.ActiveDriver.ResourceManager.LoadFont(basePath + "resources/fonts/OpenSans.ttf");

            GUI.Instance = new GUI();

            GUI.Instance.Skin = new crossGFX.GUI.Skins.Skin(DriverManager.ActiveDriver.ResourceManager.CreateRenderTexture(512, 512));
            Skin skin = GUI.Instance.Skin;
            //skin.SkinTexture.Fill(Color.Black);
            skin.Font = DriverManager.ActiveDriver.ResourceManager.LoadFont("fonts/OpenSans.ttf");
            ITexture defaultSkinTexture = DriverManager.ActiveDriver.ResourceManager.LoadTexture("skins/DefaultSkin/Skin.png");
            
            skin.SetTextureDirect(defaultSkinTexture, skin.TextField.TextureBaseRegion, skin.TextField.TextureBaseRegion); // can draw to skin texture directly
            skin.Button.TextureBase = DriverManager.ActiveDriver.ResourceManager.LoadTexture("skins/DefaultSkin/Button/Base.png"); // or through helper functions
            skin.SetTextureDirect(defaultSkinTexture, skin.Button.TextureHoverRegion, skin.Button.TextureHoverRegion);
            skin.SetTextureDirect(defaultSkinTexture, skin.Button.TextureDisabledRegion, skin.Button.TextureDisabledRegion);
            skin.SetTextureDirect(defaultSkinTexture, skin.Button.TextureDownRegion, skin.Button.TextureDownRegion);
            skin.SetTextureDirect(defaultSkinTexture, skin.CheckBox.TextureBaseRegion, skin.CheckBox.TextureBaseRegion);
            skin.SetTextureDirect(defaultSkinTexture, skin.CheckBox.TextureBaseActiveRegion, skin.CheckBox.TextureBaseActiveRegion);
            
            scene = new Scene();
            scene.SubscribeToEvents(window.InputHelper);

            Button button = new Button();
            button.Bounds = new Rectangle(100, 100, 100, 32);
            button.Text = "Press me!";
            scene.Actors.Add(button);

            Button button2 = new Button();
            button2.Bounds = new Rectangle(100, 200, 100, 64);
            button2.Text = "Don't press";
            button2.Enabled = false;
            scene.Actors.Add(button2);

            Label label = new Label();
            label.Bounds = new Rectangle(200, 100, 100, 30);
            label.Text = "Hello, I am a world!";
            label.TextSize = 12;
            scene.Actors.Add(label);

            TextBox textBox = new TextBox();
            textBox.Bounds = new Rectangle(300, 200, 200, 30);
            scene.Actors.Add(textBox);

            TextBox textBox2 = new TextBox();
            textBox2.Bounds = new Rectangle(300, 400, 200, 30);
            textBox2.TextureBase = DriverManager.ActiveDriver.ResourceManager.LoadTexture("skins/CustomSkin/TextBox/Base.png");
            textBox2.Text = "Masked text";
            textBox2.Mask = '*';
            scene.Actors.Add(textBox2);

            CheckBox checkBox = new CheckBox();
            checkBox.Bounds = new Rectangle(10, 10, 15, 15);
            scene.Actors.Add(checkBox);
        }

        static void window_OnDraw(object sender, TickEventArgs e) {
            IWindow window = sender as IWindow;
            // Do whatever drawing we want to do here

            window.Fill(Color.SkyBlue);

            scene.Draw(window);
        }

        static void button_Click(object sender, EventArgs e) {
           
        }

        static void InputHelper_KeyPressed(object sender, crossGFX.Input.KeyEventArgs e) {
            //Console.WriteLine("::Key Pressed: " + e.Code);
            switch (e.Code) {
                case crossGFX.Input.Key.Left:
                    shipPosition.X -= 5;
                    break;
                case crossGFX.Input.Key.Right:
                    shipPosition.X += 5;
                    break;
                case crossGFX.Input.Key.Up:
                    shipPosition.Y -= 5;
                    break;
                case crossGFX.Input.Key.Down:
                    shipPosition.Y += 5;
                    break;
            }
        }

        static void AddEnvironmentPaths(string[] paths) {
            string path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            path += ";" + string.Join(";", paths);

            Environment.SetEnvironmentVariable("PATH", path);

        }
    }
}
