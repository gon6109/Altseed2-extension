using Altseed2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altseed2Extension.Tool
{
    public enum ToolElementType
    {
        Bool,
        Color,
        Float,
        Group,
        InputText,
        Int,
        Label,
        List,
        Path,
        Vector2F,
        TextureBase,
        Font,
    }

    public class ToolElementManager
    {

        public class ObjectMapping
        {
            public ToolElementType ToolElementType { get; }

            public string Name { get; }

            public string PropertyName { get; }

            public Dictionary<string, object> Options { get; }

            public ObjectMapping(ToolElementType toolElementType, string name, string propertyName, Dictionary<string, object> options)
            {
                ToolElementType = toolElementType;
                Name = name;
                PropertyName = propertyName;
                Options = options ?? new Dictionary<string, object>();
            }
        }

        private static Dictionary<Type, List<ObjectMapping>> objectMappings = new Dictionary<Type, List<ObjectMapping>>();

        public static bool AddObjectMapping(Type type, IEnumerable<ObjectMapping> objectMappings)
        {
            foreach (var objectMapping in objectMappings)
            {
                try
                {
                    _ = type.GetProperty(objectMapping.PropertyName);
                }
                catch
                {
                    return false;
                }
            }
            ToolElementManager.objectMappings[type] = objectMappings.ToList();
            return true;
        }

        public static IEnumerable<ToolElement> CreateToolElements(object source)
        {
            Dictionary<string, ObjectMapping> GetObjectMappings(Type type)
            {
                var objectMappings = new Dictionary<string, ObjectMapping>();
                if (type.BaseType != null)
                {
                    var baseMappings = GetObjectMappings(type.BaseType);
                    foreach (var item in baseMappings)
                    {
                        objectMappings[item.Key] = item.Value;
                    }
                }

                foreach (var interface_ in type.GetInterfaces())
                {
                    var interfaceMappings = GetObjectMappings(type.BaseType);
                    foreach (var item in interfaceMappings)
                    {
                        objectMappings[item.Key] = item.Value;
                    }
                }

                if (ToolElementManager.objectMappings.ContainsKey(type))
                {
                    var temp = ToolElementManager.objectMappings[type].ToDictionary(obj => obj.Name, obj => obj);

                    foreach (var item in temp)
                    {
                        objectMappings[item.Key] = temp[item.Key];
                    }
                }

                return objectMappings;
            }
            var type = source.GetType();
            var objectMappings = GetObjectMappings(type);

            List<ToolElement> res = new List<ToolElement>();
            try
            {
                foreach (var objectMapping in objectMappings.Values)
                {
                    ToolElement toolElement = null;
                    switch (objectMapping.ToolElementType)
                    {
                        case ToolElementType.Bool:
                            toolElement = BoolToolElement.Create(source, objectMapping);
                            break;
                        case ToolElementType.Color:
                            toolElement = ColorToolElement.Create(source, objectMapping);
                            break;
                        case ToolElementType.Group:
                            toolElement = GroupToolElement.Create(source, objectMapping);
                            break;
                        case ToolElementType.InputText:
                            toolElement = InputTextToolElement.Create(source, objectMapping);
                            break;
                        case ToolElementType.Int:
                            toolElement = IntToolElement.Create(source, objectMapping);
                            break;
                        case ToolElementType.Label:
                            toolElement = LabelToolElement.Create(source, objectMapping);
                            break;
                        case ToolElementType.List:
                            toolElement = ListToolElement.Create(source, objectMapping);
                            break;
                        case ToolElementType.Path:
                            toolElement = PathToolElement.Create(source, objectMapping);
                            break;
                        case ToolElementType.Vector2F:
                            toolElement = Vector2FToolElement.Create(source, objectMapping);
                            break;
                        case ToolElementType.Float:
                            toolElement = FloatToolElement.Create(source, objectMapping);
                            break;
                        case ToolElementType.TextureBase:
                            toolElement = TextureBaseToolElement.Create(source, objectMapping);
                            break;
                        case ToolElementType.Font:
                            toolElement = FontToolElement.Create(source, objectMapping);
                            break;
                        default:
                            Engine.Log.Error(LogCategory.User, $"{objectMapping.ToolElementType} is not defined.");
                            break;
                    }
                    if (toolElement != null)
                        res.Add(toolElement);
                }
            }
            catch (Exception e)
            {
                Engine.Log.Error(LogCategory.User, e.Message);
                Engine.Log.Error(LogCategory.User, e.StackTrace);
                return null;
            }

            return res;
        }

        public static void SetAltseed2DefaultObjectMapping()
        {
            objectMappings.Add(typeof(TransformNode),
                new List<ObjectMapping>
                {
                    new ObjectMapping(ToolElementType.Vector2F, "Position", "Position", null),
                    new ObjectMapping(ToolElementType.Vector2F, "CenterPosition", "CenterPosition", null),
                    new ObjectMapping(ToolElementType.Vector2F, "Scale", "Scale", new Dictionary<string, object>() { { "speed", 0.1f }, { "min", 0f}, {"max", 1f} }),
                    new ObjectMapping(ToolElementType.Float, "Angle", "Angle", new Dictionary<string, object>() { {"min", -180f}, {"max", 180f} }),
                    new ObjectMapping(ToolElementType.Bool, "HorizontalFlip", "HorizontalFlip", null),
                    new ObjectMapping(ToolElementType.Bool, "VerticalFlip", "VerticalFlip", null),
                });

            objectMappings.Add(typeof(TextNode),
                new List<ObjectMapping>
                {
                    new ObjectMapping(ToolElementType.InputText, "Text", "Text", null),
                    new ObjectMapping(ToolElementType.Font, "Font", "Font", null),
                    new ObjectMapping(ToolElementType.Color, "Color", "Color", null),
                    new ObjectMapping(ToolElementType.Bool, "IsEnableKerning", "IsEnableKerning", null),
                    new ObjectMapping(ToolElementType.Float, "CharacterSpace", "CharacterSpace", null),
                    new ObjectMapping(ToolElementType.Float, "Weight", "Weight", null),
                    new ObjectMapping(ToolElementType.Int, "ZOrder", "ZOrder", null),
                    new ObjectMapping(ToolElementType.Bool, "IsDrawn", "IsDrawn", null),
                });

            objectMappings.Add(typeof(SpriteNode),
                new List<ObjectMapping>
                {
                    new ObjectMapping(ToolElementType.TextureBase, "Texture", "Texture", null),
                    new ObjectMapping(ToolElementType.Color, "Color", "Color", null),
                    new ObjectMapping(ToolElementType.Int, "ZOrder", "ZOrder", null),
                    new ObjectMapping(ToolElementType.Bool, "IsDrawn", "IsDrawn", null),
                });

            objectMappings.Add(typeof(ShapeNode),
                new List<ObjectMapping>
                {
                    new ObjectMapping(ToolElementType.TextureBase, "Texture", "Texture", null),
                    new ObjectMapping(ToolElementType.Int, "ZOrder", "ZOrder", null),
                    new ObjectMapping(ToolElementType.Bool, "IsDrawn", "IsDrawn", null),
                });

            objectMappings.Add(typeof(ArcNode),
                new List<ObjectMapping>
                {
                    new ObjectMapping(ToolElementType.Color, "Color", "Color", null),
                    new ObjectMapping(ToolElementType.Float, "Radius", "Radius", null),
                    new ObjectMapping(ToolElementType.Float, "StartDegree", "StartDegree", new Dictionary<string, object>() { { "min", -180f }, {"max", 180f } }),
                    new ObjectMapping(ToolElementType.Float, "EndDegree", "EndDegree", new Dictionary<string, object>() { { "min", -180f }, {"max", 180f } }),
                    new ObjectMapping(ToolElementType.Int, "VertNum", "VertNum", new Dictionary<string, object>() { { "min", 3 }, {"max", 500 } }),
                });

            objectMappings.Add(typeof(CircleNode),
                new List<ObjectMapping>
                {
                    new ObjectMapping(ToolElementType.Color, "Color", "Color", null),
                    new ObjectMapping(ToolElementType.Float, "Radius", "Radius", null),
                    new ObjectMapping(ToolElementType.Int, "VertNum", "VertNum", new Dictionary<string, object>() { { "min", 3 }, {"max", 500 } }),
                });

            objectMappings.Add(typeof(LineNode),
                new List<ObjectMapping>
                {
                    new ObjectMapping(ToolElementType.Color, "Color", "Color", null),
                    new ObjectMapping(ToolElementType.Float, "Thickness", "Thickness", null),
                    new ObjectMapping(ToolElementType.Vector2F, "Point1", "Point1", null),
                    new ObjectMapping(ToolElementType.Vector2F, "Point2", "Point2", null),
                });

            objectMappings.Add(typeof(RectangleNode),
                new List<ObjectMapping>
                {
                    new ObjectMapping(ToolElementType.Color, "Color", "Color", null),
                    new ObjectMapping(ToolElementType.Vector2F, "RectangleSize", "RectangleSize", null),
                });

            objectMappings.Add(typeof(TriangleNode),
                new List<ObjectMapping>
                {
                    new ObjectMapping(ToolElementType.Color, "Color", "Color", null),
                    new ObjectMapping(ToolElementType.Vector2F, "Point1", "Point1", null),
                    new ObjectMapping(ToolElementType.Vector2F, "Point2", "Point2", null),
                    new ObjectMapping(ToolElementType.Vector2F, "Point3", "Point3", null),
                });
        }
    }
}