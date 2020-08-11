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
            Engine.Tool.AddFontFromFileTTF("../TestData/Font/mplus-1m-regular.ttf", 20, ToolGlyphRange.Japanese);
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
            UpdateNodeTreeWindow();
            UpdateSelectedWindow();
            return Engine.Update();
        }

        private static void UpdateNodeTreeWindow()
        {
            void NodeTree(IEnumerable<Altseed2.Node> nodes)
            {
                foreach (var node in nodes)
                {
                    Engine.Tool.PushID(node.GetHashCode());
                    var flags = ToolTreeNodeFlags.OpenOnArrow;
                    if (node == Selected)
                        flags |= ToolTreeNodeFlags.Selected;
                    if (node.Children.Count == 0)
                        flags |= ToolTreeNodeFlags.Leaf;

                    bool treeNode = Engine.Tool.TreeNodeEx(node.ToString(), flags);
                    if (Engine.Tool.IsItemClicked(0))
                    {
                        Selected = node;
                    }

                    if (treeNode)
                    {
                        NodeTree(node.Children);
                        Engine.Tool.TreePop();
                    }
                    Engine.Tool.PopID();
                }
            }

            var size = new Vector2F(300, Engine.WindowSize.Y - 15);
            var pos = new Vector2F(0, 15);
            Engine.Tool.SetNextWindowSize(size, ToolCond.None);
            Engine.Tool.SetNextWindowPos(pos, ToolCond.None);
            var flags = ToolWindowFlags.NoMove | ToolWindowFlags.NoBringToFrontOnFocus
                | ToolWindowFlags.NoResize | ToolWindowFlags.NoScrollbar
                | ToolWindowFlags.NoScrollbar | ToolWindowFlags.NoCollapse;

            if (Engine.Tool.Begin("Node", flags))
            {
                if (Engine.Tool.Button("Sprite"))
                    Engine.AddNode(new SpriteNode());
                Engine.Tool.SameLine();
                if (Engine.Tool.Button("Text"))
                    Engine.AddNode(new TextNode());
                Engine.Tool.SameLine();
                if (Engine.Tool.Button("Arc"))
                    Engine.AddNode(new ArcNode() { Radius = 100, StartDegree = 0, EndDegree = 90 });
                Engine.Tool.SameLine();
                if (Engine.Tool.Button("Circle"))
                    Engine.AddNode(new CircleNode() { Radius = 100 });
                Engine.Tool.SameLine();
                if (Engine.Tool.Button("Line"))
                    Engine.AddNode(new LineNode() { Point2 = new Vector2F(100, 100) });
                if (Engine.Tool.Button("Rectangle"))
                    Engine.AddNode(new RectangleNode() { RectangleSize = new Vector2F(100, 100) });
                Engine.Tool.SameLine();
                if (Engine.Tool.Button("Triangle"))
                    Engine.AddNode(new TriangleNode() { Point2 = new Vector2F(50, 50), Point3 = new Vector2F(100, 0) });
               
                NodeTree(Engine.GetNodes());
                Engine.Tool.End();
            }
        }

        private static void UpdateSelectedWindow()
        {
            var size = new Vector2F(300, Engine.WindowSize.Y - 15);
            var pos = new Vector2F(Engine.WindowSize.X - size.X, 15);
            Engine.Tool.SetNextWindowSize(size, ToolCond.None);
            Engine.Tool.SetNextWindowPos(pos, ToolCond.None);
            var flags = ToolWindowFlags.NoMove | ToolWindowFlags.NoBringToFrontOnFocus
                | ToolWindowFlags.NoResize | ToolWindowFlags.NoScrollbar
                | ToolWindowFlags.NoScrollbar | ToolWindowFlags.NoCollapse;

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
