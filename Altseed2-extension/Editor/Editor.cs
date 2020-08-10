using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Editor
{
    public static class Editor
    {
        private static object selected;

        private static IEnumerable<Tool.ToolElement> selectedToolElements;

        public static object Selected
        {
            get => selected;
            set
            {
                if (value == selected)
                    return;
                selected = value;

                selectedToolElements = Tool.ToolElementManager.CreateToolElements(selected);
            }
        }

        public static bool Initialize(string title, int width, int height, Configuration config = null)
        {
            if (config == null)
                config = new Configuration();
            config.ToolEnabled = true;

            var res = Engine.Initialize(title, width, height, config);
            Tool.ToolElementManager.SetAltseed2DefaultObjectMapping();
            return res;
        }

        public static void Terminate()
        {
            Engine.Terminate();
        }

        public static bool Update()
        {
            UpdateMenu();
            UpdateSelectedWindow();
            return Engine.Update();
        }

        private static void UpdateSelectedWindow()
        {
            var size = new Vector2F(300, Engine.WindowSize.Y - 15);
            var pos = new Vector2F(Engine.WindowSize.X - size.X, 15);
            Engine.Tool.SetNextWindowSize(size, ToolCond.None);
            Engine.Tool.SetNextWindowPos(pos, ToolCond.None);
            var flags = ToolWindowFlags.NoMove | ToolWindowFlags.NoBringToFrontOnFocus
                | ToolWindowFlags.NoResize | ToolWindowFlags.NoScrollbar
                | ToolWindowFlags.NoScrollbar | ToolWindowFlags.NoTitleBar;

            if (Engine.Tool.Begin("Selected", flags))
            {
                if (selectedToolElements != null)
                {
                    Engine.Tool.PushID("Selected".GetHashCode());
                    foreach (var toolElement in selectedToolElements)
                    {
                        Engine.Tool.PushID(toolElement.GetHashCode());
                        toolElement.Update();
                        Engine.Tool.PopID();
                    }
                    Engine.Tool.PopID();
                }
                Engine.Tool.End();
            }
        }

        static void UpdateMenu()
        {
            if (Engine.Tool.BeginMainMenuBar())
            {
                if (Engine.Tool.BeginMenu("File", true))
                {
                    Engine.Tool.MenuItem("New", "", false, true);
                    Engine.Tool.MenuItem("Open", "", false, true);
                    Engine.Tool.EndMenu();
                }
                if (Engine.Tool.BeginMenu("Edit", true))
                {
                    Engine.Tool.MenuItem("Copy", "", false, true);
                    Engine.Tool.MenuItem("Paste", "", false, true);
                    Engine.Tool.EndMenu();
                }
                Engine.Tool.EndMainMenuBar();
            }
        }
    }
}
