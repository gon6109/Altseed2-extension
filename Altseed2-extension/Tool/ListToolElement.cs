using Altseed2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class ListToolElement : ToolElement
    {
        private int current = -1;

        public string ListElementPropertyName { get; }
        public string AddMethodName { get; }
        public string RemoveMethodName { get; }
        public int Current { get => current; set => current = value; }

        public MethodInfo AddMethodInfo
        {
            get
            {
                try
                {
                    return Source?.GetType().GetMethod(AddMethodName);
                }
                catch
                {
                    return null;
                }
            }
        }

        public MethodInfo RemoveMethodInfo
        {
            get
            {
                try
                {
                    return Source?.GetType().GetMethod(RemoveMethodName);
                }
                catch
                {
                    return null;
                }
            }
        }

        public ListToolElement(string name, object source, string propertyName, string listElementPropertyName = null, string addMethodName = null, string removeMethodName = null) : base(name, source, propertyName)
        {
            ListElementPropertyName = listElementPropertyName;
            AddMethodName = addMethodName;
            RemoveMethodName = removeMethodName;

            if (PropertyInfo.PropertyType.GetGenericTypeDefinition() != typeof(List<>))
            {
                throw new ArgumentException("参照先がList<>型ではありません");
            }

            if (ListElementPropertyName != null && !typeof(string).IsAssignableFrom(PropertyInfo.PropertyType.GetGenericArguments()[0].GetProperty(ListElementPropertyName).PropertyType))
            {
                throw new ArgumentException("Listの各要素の表示名がstring型ではありません");
            }
        }

        public override void Update()
        {
            base.Update();

            if (Source == null || PropertyInfo == null) return;

            IList list = (IList)PropertyInfo.GetValue(Source);
            List<string> names;
            if (ListElementPropertyName == null)
            {
                names = list.OfType<object>().Select(obj => obj.ToString()).ToList();
            }
            else
            {
                names = list.OfType<object>().Select(obj => (string)obj.GetType().GetProperty(ListElementPropertyName).GetValue(obj)).ToList();
            }

            Engine.Tool.ListBox(Name, ref current, names, names.Count);

            if (Engine.Tool.SmallButton("+"))
            {
                AddMethodInfo.Invoke(Source, new object[] { Current });
            }

            if (Engine.Tool.SmallButton("-"))
            {
                RemoveMethodInfo.Invoke(Source, new object[] { Current });
            }
        }

        public static ListToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            var listElementPropertyName = objectMapping.Options.ContainsKey("listElementPropertyName") ? (string)objectMapping.Options["listElementPropertyName"] : null;
            var addMethodName = objectMapping.Options.ContainsKey("addMethodName") ? (string)objectMapping.Options["addMethodName"] : null;
            var removeMethodName = objectMapping.Options.ContainsKey("removeMethodName") ? (string)objectMapping.Options["removeMethodName"] : null;
            return new ListToolElement(objectMapping.Name, source, objectMapping.PropertyName, listElementPropertyName, addMethodName, removeMethodName);
        }
    }
}
