// Copyright (c) 2014 CrossGFX Team

// This is free and unencumbered software released into the public domain.

// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.

// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

// For more information, please refer to <http://unlicense.org/>

using System;
using System.Collections.Generic;
using System.IO;
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
            DriverManager.ActiveDriver.ResourceManager.PrepareResourceDirectory(Path.Combine(basePath, "resources"));

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
            window.InputHelper.KeyRepeatDelay = 0;
            window.InputHelper.KeyRepeatRate = 0;

            texture = DriverManager.ActiveDriver.ResourceManager.LoadTexture("sprites/testsprite.png");
            font = DriverManager.ActiveDriver.ResourceManager.LoadFont("fonts/OpenSans.ttf");

            GUI.Instance = new GUI();

            GUI.Instance.Skin = new crossGFX.GUI.Skins.Skin(DriverManager.ActiveDriver.ResourceManager.CreateRenderTexture(512, 512));
            Skin skin = GUI.Instance.Skin;
            //skin.SkinTexture.Fill(Color.Black);
            skin.Font = DriverManager.ActiveDriver.ResourceManager.LoadFont("fonts/OpenSans.ttf");
            ITexture defaultSkinTexture = DriverManager.ActiveDriver.ResourceManager.LoadTexture("skins/DefaultSkin/Skin.png");

            skin.SetTextureDirect(defaultSkinTexture, skin.TextField.TextureBaseRegion, skin.TextField.TextureBaseRegion); // can draw to skin texture directly
            skin.TextField.TextureDisabled = CropTexture(defaultSkinTexture, skin.TextField.TextureDisabledRegion, "TextBox/Disabled"); // or through helper functions
            skin.Button.TextureBase = DriverManager.ActiveDriver.ResourceManager.LoadTexture("skins/DefaultSkin/Button/Base.png");
            #region Textures
            skin.Button.TextureHover = CropTexture(defaultSkinTexture, skin.Button.TextureHoverRegion, "Button/Hover");
            skin.Button.TextureDisabled = CropTexture(defaultSkinTexture, skin.Button.TextureDisabledRegion, "Button/Disabled");
            skin.Button.TextureDown = CropTexture(defaultSkinTexture, skin.Button.TextureDownRegion, "Button/Down");
            skin.CheckBox.TextureBase = CropTexture(defaultSkinTexture, skin.CheckBox.TextureBaseRegion, "CheckBox/Base");
            skin.CheckBox.TextureBaseActive = CropTexture(defaultSkinTexture, skin.CheckBox.TextureBaseActiveRegion, "CheckBox/Active");
            skin.ScrollBarVertical.TextureBase = CropTexture(defaultSkinTexture, skin.ScrollBarVertical.TextureBaseRegion, "ScrollBar/Vertical/Base");
            skin.ScrollBarVertical.TextureHover = CropTexture(defaultSkinTexture, skin.ScrollBarVertical.TextureHoverRegion, "ScrollBar/Vertical/Hover");
            skin.ScrollBarVertical.TextureDisabled = CropTexture(defaultSkinTexture, skin.ScrollBarVertical.TextureDisabledRegion, "ScrollBar/Vertical/Disabled");
            skin.ScrollBarVertical.TextureDown = CropTexture(defaultSkinTexture, skin.ScrollBarVertical.TextureDownRegion, "ScrollBar/Vertical/Base");
            skin.ScrollBarButtonUp.TextureBase = CropTexture(defaultSkinTexture, skin.ScrollBarButtonUp.TextureBaseRegion, "ScrollBar/Vertical/Button/Up/Base");
            skin.ScrollBarButtonUp.TextureHover = CropTexture(defaultSkinTexture, skin.ScrollBarButtonUp.TextureHoverRegion, "ScrollBar/Vertical/Button/Up/Hover");
            skin.ScrollBarButtonUp.TextureDisabled = CropTexture(defaultSkinTexture, skin.ScrollBarButtonUp.TextureDisabledRegion, "ScrollBar/Vertical/Button/Up/Disabled");
            skin.ScrollBarButtonUp.TextureDown = CropTexture(defaultSkinTexture, skin.ScrollBarButtonUp.TextureDownRegion, "ScrollBar/Vertical/Button/Up/Down");
            skin.ScrollBarButtonDown.TextureBase = CropTexture(defaultSkinTexture, skin.ScrollBarButtonDown.TextureBaseRegion, "ScrollBar/Vertical/Button/Down/Base");
            skin.ScrollBarButtonDown.TextureHover = CropTexture(defaultSkinTexture, skin.ScrollBarButtonDown.TextureHoverRegion, "ScrollBar/Vertical/Button/Down/Hover");
            skin.ScrollBarButtonDown.TextureDisabled = CropTexture(defaultSkinTexture, skin.ScrollBarButtonDown.TextureDisabledRegion, "ScrollBar/Vertical/Button/Down/Disabled");
            skin.ScrollBarButtonDown.TextureDown = CropTexture(defaultSkinTexture, skin.ScrollBarButtonDown.TextureDownRegion, "ScrollBar/Vertical/Button/Down/Down");
            skin.ScrollBarButtonLeft.TextureBase = CropTexture(defaultSkinTexture, skin.ScrollBarButtonLeft.TextureBaseRegion, "ScrollBar/Horizontal/Button/Left/Base");
            skin.ScrollBarButtonLeft.TextureHover = CropTexture(defaultSkinTexture, skin.ScrollBarButtonLeft.TextureHoverRegion, "ScrollBar/Horizontal/Button/Left/Hover");
            skin.ScrollBarButtonLeft.TextureDisabled = CropTexture(defaultSkinTexture, skin.ScrollBarButtonLeft.TextureDisabledRegion, "ScrollBar/Horizontal/Button/Left/Disabled");
            skin.ScrollBarButtonLeft.TextureDown = CropTexture(defaultSkinTexture, skin.ScrollBarButtonLeft.TextureDownRegion, "ScrollBar/Horizontal/Button/Left/Down");
            skin.ScrollBarButtonRight.TextureBase = CropTexture(defaultSkinTexture, skin.ScrollBarButtonRight.TextureBaseRegion, "ScrollBar/Horizontal/Button/Right/Base");
            skin.ScrollBarButtonRight.TextureHover = CropTexture(defaultSkinTexture, skin.ScrollBarButtonRight.TextureHoverRegion, "ScrollBar/Horizontal/Button/Right/Hover");
            skin.ScrollBarButtonRight.TextureDisabled = CropTexture(defaultSkinTexture, skin.ScrollBarButtonRight.TextureDisabledRegion, "ScrollBar/Horizontal/Button/Right/Disabled");
            skin.ScrollBarButtonRight.TextureDown = CropTexture(defaultSkinTexture, skin.ScrollBarButtonRight.TextureDownRegion, "ScrollBar/Horizontal/Button/Right/Down");

            skin.TextField.TextureBase = CropTexture(defaultSkinTexture, skin.TextField.TextureBaseActiveRegion, "TextBox/Base");
            #endregion


            scene = new Scene();
            scene.SubscribeToEvents(window.InputHelper);

            Panel panel = new Panel();
            panel.Bounds = new Rectangle(250, 25, 500, 150);
            panel.BackgroundColor = new Color(31, 255, 255, 255);
            scene.Add(panel);

            Label label = new Label();
            label.Bounds = new Rectangle(0, 0, 100, 30);
            label.Value.Text = "Hello, I am a world!";
            panel.Add(label);

            ScrollBar labelScrollBar = new ScrollBar(ScrollBar.ScrollBarType.Horizontal);
            labelScrollBar.Bounds = new Rectangle(0, 50, 100, 15);
            labelScrollBar.Target = label;
            labelScrollBar.StepSize = 12;
            panel.Add(labelScrollBar);

            TextBox textBox = new TextBox();
            textBox.Bounds = new Rectangle(150, 10, 200, 30);
            panel.Add(textBox);

            TextBox textBox2 = new TextBox();
            textBox2.Bounds = new Rectangle(150, 60, 200, 30);
            textBox2.TextureBase = DriverManager.ActiveDriver.ResourceManager.LoadTexture("skins/CustomSkin/TextBox/Base.png");
            textBox2.Value.Text = "Masked text";
            textBox2.Mask = '*';
            panel.Add(textBox2);

            CheckBox checkBox = new CheckBox();
            checkBox.Bounds = new Rectangle(380, 15, 15, 15);
            panel.Add(checkBox);

            Button button = new Button();
            button.Bounds = new Rectangle(400, 10, 100, 30);
            button.Value.Text = "Press me!";
            panel.Add(button);

            Button button2 = new Button();
            button2.Bounds = new Rectangle(400, 60, 100, 64);
            button2.Value.Text = "Don't press";
            button2.Enabled = false;
            panel.Add(button2);

            ListBox<object> listBox = new ListBox<object>(ListBox<object>.SelectionMode.Multiple);
            {
                listBox.Bounds = new Rectangle(550, 200, 100, 200);
                listBox.List.Add("Element");
                listBox.List.Add("Another element");
                listBox.SetLabel(listBox.List.Count - 1, "A custom label");
                listBox.List.Add(69);
                for (int i = 0; i < 20; i++) {
                    listBox.List.Add(i);
                }
            }
            scene.Add(listBox);

            ScrollBar listBoxScrollBar = new ScrollBar(ScrollBar.ScrollBarType.Vertical);
            listBoxScrollBar.Bounds = new Rectangle(650, 200, 15, 200);
            listBoxScrollBar.Target = listBox;
            listBoxScrollBar.StepSize = 12;
            scene.Add(listBoxScrollBar);

            RichLabel richLabel = new RichLabel();
            richLabel.Bounds = new Rectangle(300, 200, 200, 200);
            richLabel.Fonts.Add(DriverManager.ActiveDriver.ResourceManager.LoadFont("fonts/ComicSans.ttf"));
            richLabel.Colors.Add(new Color(255, 0, 255, 0));
            richLabel.Append("<fs24>Hello, I am a world!<line>This is a new line<line>");
            richLabel.Append("<fs18>This <fs20>line <fs20>has <fs48>different <fs24>text <fs18>sizes.<line>");
            richLabel.Append("<fs24><b>This<b0> <i>is<i0> <u>stylized<u0> text.<line>");
            richLabel.Append("<fcFF0000>This <cf1>line <fc0000FF>is <fc000000>multicolored.<line>");
            richLabel.Append("<f1>This line is <i>awful<i0><f0><line>");
            richLabel.Append("This line is sooo long, it has to be wrapped to make it fit in the horizontal boundary.<line>");
            richLabel.Append("This is the bottom line.");
            scene.Add(richLabel);

            RichLabel chatLabel = new RichLabel();
            chatLabel.Bounds = new Rectangle(0, 0, 185, 570);
            chatLabel.Append("<fs24> Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam consequat sed lorem id aliquet. Nulla suscipit, nulla quis aliquam lobortis, magna risus accumsan dui, quis gravida elit nibh vel justo. Nulla at nunc dapibus, pellentesque nibh sit amet, cursus ante. Ut a mi felis. Aliquam iaculis lorem risus, quis pharetra nisl accumsan sit amet. Curabitur malesuada diam metus, id eleifend sem varius sit amet. Praesent sodales feugiat ipsum, id varius quam dapibus sed. Vestibulum tempus sapien arcu, quis mattis nunc laoreet sed. Morbi in massa lectus. Ut ipsum tortor, feugiat eu nulla eget, hendrerit dignissim diam. Suspendisse potenti. Nulla vitae odio nec diam malesuada condimentum. Nulla erat magna, interdum posuere varius a, sagittis eu massa. Aenean sit amet lectus lorem. Vestibulum eu diam luctus, bibendum urna in, mollis arcu. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus dapibus, enim sed porttitor eleifend, enim dui posuere erat, sed ullamcorper arcu urna vitae leo. Donec rutrum at turpis ut scelerisque. Etiam non posuere lacus, nec blandit augue. Phasellus dignissim sapien at nunc rutrum, et ultricies augue congue. Maecenas pellentesque, lorem sed pulvinar ullamcorper, erat velit molestie lacus, vel imperdiet erat nibh a tellus. Maecenas non dolor pellentesque, elementum ipsum quis, aliquet metus. Praesent ac mauris facilisis, viverra quam id, sagittis lacus. Mauris id vulputate tellus. Nulla dapibus convallis leo, sit amet interdum nisi mollis et. Phasellus feugiat, nunc sit amet rutrum accumsan, velit urna tempus enim, eu facilisis sem eros id diam. In hac habitasse platea dictumst. Curabitur magna elit, elementum et eros vel, congue pharetra nibh. Duis vel consequat leo, at varius ipsum. In condimentum volutpat orci, sed commodo magna cursus vitae. Donec at condimentum enim. Sed vulputate nibh quis scelerisque vulputate. Duis mattis massa consequat, convallis dui nec, viverra massa. Quisque eu dui fringilla, vestibulum dui id, ornare tortor. Mauris varius tempus tempor. Mauris nec nibh molestie nisl rhoncus pellentesque in eu enim. Nam feugiat risus nulla, sit amet congue tellus aliquam nec. Mauris id elementum sapien. Sed placerat nulla non dui ultricies, et vulputate metus iaculis. Nunc congue enim quis arcu aliquam fringilla. Cras ut neque velit. Phasellus tristique, dolor a auctor luctus, risus sapien dignissim est, sit amet pretium ipsum dui commodo mi. Nulla egestas leo sit amet gravida placerat. Nullam eu venenatis lacus. Curabitur sagittis purus vel velit ultrices volutpat. Fusce quis tellus quis mauris iaculis elementum aliquam nec enim. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Phasellus in nulla quis nulla placerat rutrum. Suspendisse luctus lacinia metus, ut iaculis elit ultricies vel. Maecenas venenatis massa at mauris luctus accumsan. Vestibulum vitae lorem ut risus semper tristique. Quisque posuere, arcu at sagittis egestas, neque sapien interdum massa, non blandit sapien mauris dictum metus. Sed sodales tempus ante, sed facilisis tellus suscipit quis. Morbi eu odio lacinia, blandit lacus non, mollis dui. Vivamus pharetra ligula convallis tortor elementum iaculis. Fusce suscipit cursus erat eu cursus. Maecenas vulputate, erat sed accumsan cursus, nibh metus dapibus ipsum, nec scelerisque nisl mauris vel nulla. Aenean euismod, tortor a lobortis sagittis, mi mauris tristique dui, ut elementum lacus neque vitae est. Nullam quis lectus ac sapien lacinia sollicitudin. Donec ultricies, odio eu ultrices faucibus, nisi ante ultricies libero, quis aliquet ligula ante a quam.");
            scene.Add(chatLabel);

            TextBox chatBox = new TextBox();
            chatBox.Bounds = new Rectangle(0, 570, 200, 30);
            chatBox.Return += new crossGFX.GUI.TextBox.ReturnHandler(chatLabel.Append);
            scene.Add(chatBox);

            ScrollBar chatScrollBar = new ScrollBar(ScrollBar.ScrollBarType.Vertical);
            chatScrollBar.Bounds = new Rectangle(185, 15, 15, 540);
            chatScrollBar.Target = chatLabel;
            chatScrollBar.StepSize = 12;
            scene.Add(chatScrollBar);

            scene.Add(chatScrollBar.CreateButtonUp());

            scene.Add(chatScrollBar.CreateButtonDown());

            Panel[] panels = new Panel[] { new Panel(), new Panel(), new Panel(), new Panel() };

            panels[0].Bounds = new Rectangle(300, 400, 192, 192);
            scene.Add(panels[0]);

            for (int i = 1; i <= 3; i++) {
                panels[i].Bounds = new Rectangle(32 * (i - 1), 32 * (i - 1), 128, 128);
                int[] color = new int[] { 127, 0, 0, 0 };
                color[i] = 255;
                panels[i].BackgroundColor = new Color(color[0], color[1], color[2], color[3]);
                panels[0].Add(panels[i]);

                string[] names = new string[] { "Top", "Bottom", "Up", "Down" };
                Action[] functions = new Action[] { panels[i].MoveTop, panels[i].MoveBottom, panels[i].MoveUp, panels[i].MoveDown };
                for (int k = 0; k < 4; k++) {
                    Button layerButton = new Button();
                    layerButton.Bounds = new Rectangle((k % 2 == 0 ? 16 : 80), (k < 2 ? 16 : 80), 32, 32);
                    layerButton.Value.Text = names[k];
                    layerButton.Value.TextSize = 8;
                    int index = k;
                    layerButton.Click += (o, a) => { functions[index](); };
                    panels[i].Add(layerButton);
                }
            }

        }

        static ITexture CropTexture(ITexture texture, Rectangle bounds, string name) {
            IRenderTexture renderTexture = DriverManager.ActiveDriver.ResourceManager.CreateRenderTexture(bounds.Width, bounds.Height);
            renderTexture.DrawStretched(texture, new Rectangle(0, 0, bounds.Width, bounds.Height), bounds);
            renderTexture.Display();

            string elementPath = Path.Combine(DriverManager.ActiveDriver.ResourceManager.ResourceDirectory, "Skins", "DefaultSkin", "Elements", name + ".png");
            if (Directory.Exists(Path.GetDirectoryName(elementPath)) == false) {
                Directory.CreateDirectory(Path.GetDirectoryName(elementPath));
            }
            renderTexture.Texture.Save(elementPath);

            return renderTexture.Texture;
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
