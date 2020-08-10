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
        Vector2F
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
            var type = source.GetType();
            var objectMappings = new Dictionary<string, ObjectMapping>();
            while (type.BaseType != null)
            {
                if (ToolElementManager.objectMappings.ContainsKey(type))
                {
                    var temp = ToolElementManager.objectMappings[type].ToDictionary(obj => obj.Name, obj => obj);

                    foreach (var item in temp)
                    {
                        objectMappings[item.Key] = temp[item.Key];
                    }
                }
                type = type.BaseType;
            }

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
                        default:
                            Engine.Log.Error(LogCategory.User, $"{objectMapping.ToolElementType} is not defined.");
                            break;
                    }
                    if (toolElement != null)
                        res.Add(toolElement);
                }
            }
            catch(Exception e)
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
        }
    }
}