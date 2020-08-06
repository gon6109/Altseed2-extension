using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Altseed2;

namespace Altseed2Extension.Extension
{
    public static class Texture2DExtension
    {
        public static string ErrorImage { get; set; } = "Static/error.png";

        /// <summary>
        /// テクスチャをロードする
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>テクスチャ</returns>
        public static Texture2D Load(string path)
        {
            var texture = Texture2D.Load(path);
            if (texture == null)
            {
                texture = Texture2D.Load(ErrorImage);
                Engine.Log.Error(LogCategory.User, $"'{path}' is not found.");
            }
            return texture;
        }

        /// <summary>
        /// テクスチャを非同期ロードする
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>テクスチャ</returns>
        public static async Task<Texture2D> LoadAsync(string path)
        {
            var texture = await Texture2D.LoadAsync(path);
            if (texture == null)
            {
                texture = await Texture2D.LoadAsync(ErrorImage);
                Engine.Log.Error(LogCategory.User, $"'{path}' is not found.");
            }
            return texture;
        }
    }
}
