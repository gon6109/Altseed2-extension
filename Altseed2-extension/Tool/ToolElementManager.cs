using Altseed2;
using Altseed2Extension.Tool.Attribute;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        Enum,
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
            Dictionary<string, ToolElement> res = new Dictionary<string, ToolElement>();

            if (System.Attribute.IsDefined(source.GetType(), typeof(ToolAutoAttribute)))
            {
                foreach (var (name, toolElement) in CreateToolElementsAuto(source))
                {
                    res[name] = toolElement;
                }
            }

            foreach (var (name, toolElement) in CreateToolElementsFromPropetyAttributes(source))
            {
                res[name] = toolElement;
            }

            foreach (var (name, toolElement) in CreateToolElementsFromObjectMappings(source))
            {
                res[name] = toolElement;
            }

            return res.Values.ToList();
        }

        private static Dictionary<string, ToolElement> CreateToolElementsAuto(object source)
        {
            Dictionary<string, ToolElement> res = new Dictionary<string, ToolElement>();

            foreach (var info in source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                try
                {
                    if (System.Attribute.IsDefined(info, typeof(ToolHiddenAttribute)))
                        continue;

                    if (!info.CanWrite)
                    {
                        res[info.Name] = new Tool.LabelToolElement(info.Name, source, info.Name);
                        continue;
                    }

                    switch (info.GetValue(source))
                    {
                        case bool _:
                            res[info.Name] = new Tool.BoolToolElement(info.Name, source, info.Name);
                            break;
                        case Color _:
                            res[info.Name] = new Tool.ColorToolElement(info.Name, source, info.Name);
                            break;
                        case Enum _:
                            res[info.Name] = new Tool.EnumToolElement(info.Name, source, info.Name);
                            break;
                        case float _:
                            res[info.Name] = new Tool.FloatToolElement(info.Name, source, info.Name);
                            break;
                        case Font _:
                            res[info.Name] = new Tool.FontToolElement(info.Name, source, info.Name);
                            break;
                        case string _:
                            if (info.CanWrite)
                                res[info.Name] = new Tool.InputTextToolElement(info.Name, source, info.Name);
                            break;
                        case int _:
                            res[info.Name] = new Tool.IntToolElement(info.Name, source, info.Name);
                            break;
                        case IList _:
                            res[info.Name] = new Tool.ListToolElement(info.Name, source, info.Name);
                            break;
                        case TextureBase _:
                            res[info.Name] = new Tool.TextureBaseToolElement(info.Name, source, info.Name);
                            break;
                        case Vector2F _:
                            res[info.Name] = new Tool.Vector2FToolElement(info.Name, source, info.Name);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Engine.Log.Error(LogCategory.User, e.Message);
                    Engine.Log.Error(LogCategory.User, e.StackTrace);
                }
            }

            return res;
        }

        private static Dictionary<string, ToolElement> CreateToolElementsFromPropetyAttributes(object source)
        {
            Dictionary<string, ToolElement> res = new Dictionary<string, ToolElement>();

            foreach (var info in source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                System.Attribute[] attributes = System.Attribute.GetCustomAttributes(info, typeof(System.Attribute));
                foreach (System.Attribute attribute in attributes)
                {
                    try
                    {
                        switch (attribute)
                        {
                            case ToolBoolAttribute toolBoolAttribute:
                                res[toolBoolAttribute.Name ?? info.Name] = new Tool.BoolToolElement(toolBoolAttribute.Name ?? info.Name, source, info.Name);
                                break;
                            case ToolColorAttribute toolColorAttribute:
                                res[toolColorAttribute.Name ?? info.Name] = new Tool.ColorToolElement(toolColorAttribute.Name ?? info.Name, source, info.Name, toolColorAttribute.Flags);
                                break;
                            case ToolEnumAttribute toolEnumAttribute:
                                res[toolEnumAttribute.Name ?? info.Name] = new Tool.EnumToolElement(toolEnumAttribute.Name ?? info.Name, source, info.Name);
                                break;
                            case ToolFloatAttribute toolFloatAttribute:
                                res[toolFloatAttribute.Name ?? info.Name] = new Tool.FloatToolElement(toolFloatAttribute.Name ?? info.Name, source, info.Name, toolFloatAttribute.Speed, toolFloatAttribute.Min, toolFloatAttribute.Max);
                                break;
                            case ToolFontAttribute toolFontAttribute:
                                res[toolFontAttribute.Name ?? info.Name] = new Tool.FontToolElement(toolFontAttribute.Name ?? info.Name, source, info.Name);
                                break;
                            case ToolGroupAttribute toolGroupAttribute:
                                res[toolGroupAttribute.Name ?? info.Name] = new Tool.GroupToolElement(toolGroupAttribute.Name ?? info.Name, source, info.Name);
                                break;
                            case ToolInputTextAttribute toolInputTextAttribute:
                                res[toolInputTextAttribute.Name ?? info.Name] = new Tool.InputTextToolElement(toolInputTextAttribute.Name ?? info.Name, source, info.Name, toolInputTextAttribute.IsMultiLine, toolInputTextAttribute.MaxLength);
                                break;
                            case ToolIntAttribute toolIntAttribute:
                                res[toolIntAttribute.Name ?? info.Name] = new Tool.IntToolElement(toolIntAttribute.Name ?? info.Name, source, info.Name, toolIntAttribute.Speed, toolIntAttribute.Min, toolIntAttribute.Max);
                                break;
                            case ToolLabelAttribute toolLabelAttribute:
                                res[toolLabelAttribute.Name ?? info.Name] = new Tool.LabelToolElement(toolLabelAttribute.Name ?? info.Name, source, info.Name);
                                break;
                            case ToolListAttribute toolListAttribute:
                                res[toolListAttribute.Name ?? info.Name] = new Tool.ListToolElement(toolListAttribute.Name ?? info.Name, source, info.Name, toolListAttribute.ListElementPropertyName, toolListAttribute.AddMethodName, toolListAttribute.RemoveMethodName);
                                break;
                            case ToolPathAttribute toolPathAttribute:
                                res[toolPathAttribute.Name ?? info.Name] = new Tool.PathToolElement(toolPathAttribute.Name ?? info.Name, source, info.Name, toolPathAttribute.IsDirectory, toolPathAttribute.Filter, toolPathAttribute.DefaultPath, toolPathAttribute.MaxLength);
                                break;
                            case ToolTextureBaseAttribute toolTextureBaseAttribute:
                                res[toolTextureBaseAttribute.Name ?? info.Name] = new Tool.TextureBaseToolElement(toolTextureBaseAttribute.Name ?? info.Name, source, info.Name);
                                break;
                            case ToolVector2FAttribute toolVector2FAttribute:
                                res[toolVector2FAttribute.Name ?? info.Name] = new Tool.Vector2FToolElement(toolVector2FAttribute.Name ?? info.Name, source, info.Name, toolVector2FAttribute.Speed, toolVector2FAttribute.Min, toolVector2FAttribute.Max);
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Engine.Log.Error(LogCategory.User, e.Message);
                        Engine.Log.Error(LogCategory.User, e.StackTrace);
                    }
                }
            }

            return res;
        }

        private static Dictionary<string, ToolElement> CreateToolElementsFromObjectMappings(object source)
        {
            Dictionary<string, ToolElement> res = new Dictionary<string, ToolElement>();
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

            foreach (var (name, objectMapping) in objectMappings)
            {
                try
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
                        case ToolElementType.Enum:
                            toolElement = EnumToolElement.Create(source, objectMapping);
                            break;
                        default:
                            Engine.Log.Error(LogCategory.User, $"{objectMapping.ToolElementType} is not defined.");
                            break;
                    }
                    res[name] = toolElement;
                }
                catch (Exception e)
                {
                    Engine.Log.Error(LogCategory.User, e.Message);
                    Engine.Log.Error(LogCategory.User, e.StackTrace);
                }
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
                    new ObjectMapping(ToolElementType.Enum, "WritingDirection", "WritingDirection", null),
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