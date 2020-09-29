using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altseed2Extension.Extension
{
    public static class NodeExtension
    {
        public static void SetIsDrawn(this Altseed2.Node node, bool isDrawn)
        {
            foreach (var descendant in node.EnumerateDescendants())
            {
                switch (descendant)
                {
                    case Altseed2.SpriteNode sprite:
                        sprite.IsDrawn = isDrawn;
                        break;
                    case Altseed2.TextNode text:
                        text.IsDrawn = isDrawn;
                        break;
                    case Altseed2.PolygonNode polygon:
                        polygon.IsDrawn = isDrawn;
                        break;
                    default:
                        break;
                }
            }
        }

        public static void AddZOrder(this Altseed2.Node node, int amount)
        {
            foreach (var descendant in node.EnumerateDescendants())
            {
                switch (descendant)
                {
                    case Altseed2.SpriteNode sprite:
                        sprite.ZOrder += amount;
                        break;
                    case Altseed2.TextNode text:
                        text.ZOrder += amount;
                        break;
                    case Altseed2.PolygonNode polygon:
                        polygon.ZOrder += amount;
                        break;
                    default:
                        break;
                }
            }
        }

        public static void SetAlpha(this Altseed2.Node node, byte alpha)
        {
            
            foreach (var descendant in node.EnumerateDescendants().Append(node))
            {
                switch (descendant)
                {
                    case Altseed2.SpriteNode sprite:
                        sprite.Color = SetAlpha(sprite.Color, alpha);
                        break;
                    case Altseed2.TextNode text:
                        text.Color = SetAlpha(text.Color, alpha);
                        break;
                    case Altseed2.ArcNode arc:
                        arc.Color = SetAlpha(arc.Color, alpha);
                        break;
                    case Altseed2.CircleNode circle:
                        circle.Color = SetAlpha(circle.Color, alpha);
                        break;
                    case Altseed2.RectangleNode rect:
                        rect.Color = SetAlpha(rect.Color, alpha);
                        break;
                    case Altseed2.TriangleNode triangle:
                        triangle.Color = SetAlpha(triangle.Color, alpha);
                        break;
                    default:
                        break;
                }
            }

            Altseed2.Color SetAlpha(Altseed2.Color origin, byte alpha)
            {
                var color = origin;
                color.A = alpha;
                return color;
            }
        }
    }
}
