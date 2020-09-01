using Altseed2;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class PathToolElement : ToolElement
    {
        int MaxLength { get; }
        public string Filter { get; }
        public string DefaultPath { get; }
        public bool IsDirectory { get; }

        public string RootDirectoryPathPropertyName { get; }

        public PropertyInfo RootDirectoryPathPropertyInfo
        {
            get
            {
                try
                {
                    return Source?.GetType().GetProperty(RootDirectoryPathPropertyName);
                }
                catch
                {
                    return null;
                }
            }
        }

        public PathToolElement(string name, object source, string propertyName, bool isDirectory = false, string filter = "", string defaultPath = "", int maxLength = 1024, string rootDirectoryPathPropertyName = null) : base(name, source, propertyName)
        {
            MaxLength = maxLength;
            Filter = filter;
            DefaultPath = defaultPath;
            IsDirectory = isDirectory;
            RootDirectoryPathPropertyName = rootDirectoryPathPropertyName;

            if (!typeof(string).IsAssignableFrom(PropertyInfo?.PropertyType))
            {
                throw new ArgumentException("参照先がstring型ではありません");
            }

            if (RootDirectoryPathPropertyName != null && !typeof(string).IsAssignableFrom(RootDirectoryPathPropertyInfo?.PropertyType))
            {
                throw new ArgumentException("参照先がstring型ではありません");
            }
        }

        public override void Update()
        {
            base.Update();

            if (Source == null || PropertyInfo == null) return;

            string path = (string)PropertyInfo.GetValue(Source);
            if (path == null)
                path = "";
            string newPath;
            if ((newPath = Engine.Tool.InputText(Name, path, MaxLength, ToolInputTextFlags.None)) != null)
            {
                PropertyInfo.SetValue(Source, newPath);
            }

            if (Engine.Tool.SmallButton("..."))
            {
                if (IsDirectory && (newPath = Engine.Tool.PickFolder(DefaultPath)) != null)
                {
                    SetPath(newPath);
                }
                else if ((newPath = Engine.Tool.OpenDialog(Filter, DefaultPath)) != null)
                {
                    SetPath(newPath);
                }
            }
        }

        private void SetPath(string newPath)
        {
            try
            {
                if (RootDirectoryPathPropertyInfo != null)
                {
                    var uri = new Uri((string)RootDirectoryPathPropertyInfo.GetValue(Source) + "/");
                    var newPathUri = new Uri(newPath);
                    newPath = uri.MakeRelativeUri(newPathUri).ToString();
                }
                PropertyInfo.SetValue(Source, newPath);
            }
            catch (Exception e)
            {
                Engine.Log.Error(LogCategory.User, e.Message);
                Engine.Log.Error(LogCategory.User, e.StackTrace);
            }
        }

        public static PathToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            var isDirectory = objectMapping.Options.ContainsKey("isDirectory") ? (bool)objectMapping.Options["isDirectory"] : false;
            var filter = objectMapping.Options.ContainsKey("filter") ? (string)objectMapping.Options["filter"] : "";
            var defaultPath = objectMapping.Options.ContainsKey("defaultPath") ? (string)objectMapping.Options["defaultPath"] : "";
            var maxLength = objectMapping.Options.ContainsKey("maxLength") ? (int)objectMapping.Options["maxLength"] : 1024;
            var rootDirectoryPathPropertyName = objectMapping.Options.ContainsKey("rootDirectoryPathPropertyName") ? (string)objectMapping.Options["rootDirectoryPathPropertyName"] : null;
            return new PathToolElement(objectMapping.Name, source, objectMapping.PropertyName, isDirectory, filter, defaultPath, maxLength, rootDirectoryPathPropertyName);
        }
    }
}
