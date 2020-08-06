using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Altseed2Extension.Input
{
    public class KeyConfig
    {
        public string ControllerName;
        public Dictionary<Inputs, InputMapping> InputMappings;

        public KeyConfig()
        {
            InputMappings = new Dictionary<Inputs, InputMapping>();
        }

        /// <summary>
        /// キーコンフィグを保存
        /// </summary>
        /// <param name="path">ファイル</param>
        public void SaveConfig(string path)
        {
            using (FileStream mapfile = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(mapfile, this);
            }
        }

        /// <summary>
        /// キーコンフィグをロードする
        /// </summary>
        /// <param name="path">ファイル</param>
        static public KeyConfig LoadConfig(string path)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            KeyConfig map = (KeyConfig)serializer.Deserialize(IO.IO.GetStream(path));
            return map;
        }
    }
}
