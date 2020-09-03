using Altseed2;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class UserToolElement : ToolElement
    {
        public string MethodName { get; }

        public MethodInfo MethodInfo
        {
            get
            {
                try
                {
                    return Source?.GetType().GetMethod(MethodName);
                }
                catch
                {
                    return null;
                }
            }
        }

        public UserToolElement(string name, object source, string methodName) : base(name, source, null)
        {
            MethodName = methodName;
            if (MethodInfo?.GetParameters().Length != 0)
            {
                throw new ArgumentException("参照するメソッドに引数を入れることはできません");
            }
        }

        public override void Update()
        {
            base.Update();
            Engine.Tool.PushID(MethodInfo.GetHashCode());
            MethodInfo?.Invoke(Source, new object[] { });
            Engine.Tool.PopID();
        }

        public static UserToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            return new UserToolElement(objectMapping.Name, source, objectMapping.PropertyName);
        }
    }
}
