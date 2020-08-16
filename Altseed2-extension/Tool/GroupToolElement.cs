using Altseed2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class GroupToolElement : ToolElement
    {
        List<ToolElement> ToolElements { get; set; }

        WeakReference<object> Target { get; set; }

        public GroupToolElement(string name, object source, string propertyName) : base(name, source, propertyName)
        {
            object target = PropertyInfo?.GetValue(source);
            if (target != null)
            {
                Target = new WeakReference<object>(target);
                ToolElements = ToolElementManager.CreateToolElements(target).ToList();
            }
        }

        public override void Update()
        {
            base.Update();

            if (Source == null || PropertyInfo == null) return;

            if ((Target == null && PropertyInfo?.GetValue(Source) != null) ||
                (Target != null && (!Target.TryGetTarget(out var target) || target != PropertyInfo?.GetValue(Source))))
            {
                ToolElements = ToolElementManager.CreateToolElements(PropertyInfo?.GetValue(Source)).ToList();
            }

            if (Engine.Tool.CollapsingHeader(Name, ToolTreeNodeFlags.CollapsingHeader))
            {
                foreach (var toolElement in ToolElements ?? Enumerable.Empty<ToolElement>())
                {
                    Engine.Tool.PushID(toolElement.GetHashCode());
                    toolElement.Update();
                    Engine.Tool.PopID();
                }
            }
        }

        public static GroupToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            return new GroupToolElement(objectMapping.Name, source, objectMapping.PropertyName);
        }
    }
}
