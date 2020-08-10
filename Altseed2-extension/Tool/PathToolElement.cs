﻿using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class PathToolElement : ToolElement
    {
        public string Filter { get; }
        public string DefaultPath { get; }
        public bool IsDirectory { get; }

        public PathToolElement(string name, object source, string propertyName, bool isDirectory = false, string filter = "", string defaultPath = "") : base(name, source, propertyName)
        {
            Filter = filter;
            DefaultPath = defaultPath;
            IsDirectory = isDirectory;

            if (!typeof(string).IsAssignableFrom(PropertyInfo?.PropertyType))
            {
                throw new ArgumentException("参照先がstring型ではありません");
            }
        }

        public override void Update()
        {
            base.Update();

            if (Source == null || PropertyInfo == null) return;

            string path = (string)PropertyInfo.GetValue(Source);
            string newPath;
            if ((newPath = Engine.Tool.InputText(Name, path, path.Length + 2, ToolInputTextFlags.None)) != null)
            {
                PropertyInfo.SetValue(Source, newPath);
            }

            if (Engine.Tool.SmallButton("..."))
            {
                if (IsDirectory && (newPath = Engine.Tool.PickFolder(DefaultPath)) != null)
                {
                    PropertyInfo.SetValue(Source, newPath);
                }
                else if ((newPath = Engine.Tool.OpenDialog(Filter, DefaultPath)) != null)
                {
                    PropertyInfo.SetValue(Source, newPath);
                }
            }
        }

        public static PathToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            var isDirectory = objectMapping.Options.ContainsKey("isDirectory") ? (bool)objectMapping.Options["isDirectory"] : false;
            var filter = objectMapping.Options.ContainsKey("filter") ? (string)objectMapping.Options["filter"] : "";
            var defaultPath = objectMapping.Options.ContainsKey("defaultPath") ? (string)objectMapping.Options["defaultPath"] : "";
            return new PathToolElement(objectMapping.Name, source, objectMapping.PropertyName, isDirectory, filter, defaultPath);
        }
    }
}