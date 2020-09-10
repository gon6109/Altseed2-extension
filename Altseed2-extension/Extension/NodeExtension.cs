﻿using System;
using System.Collections.Generic;
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
    }
}
