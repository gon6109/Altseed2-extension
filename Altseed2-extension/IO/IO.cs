using Altseed2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Altseed2Extension.IO
{
    /// <summary>
    /// ファイル入出力系
    /// </summary>
    public class IO
    {
        public static MemoryStream GetStream(string path)
        {
            if (path == null) throw new ArgumentNullException("path is null");
            var temp = StaticFile.Create(path);
            if (temp == null) throw new FileNotFoundException(path + "が見つかりません");
            var result = temp.Buffer;
            return new MemoryStream(result);
        }

        public static async Task<MemoryStream> GetStreamAsync(string path)
        {
            if (path == null) throw new ArgumentNullException("path is null");
            var temp = await StaticFile.CreateAsync(path);
            if (temp == null) throw new FileNotFoundException(path + "が見つかりません");
            var result = temp.Buffer;
            return new MemoryStream(result);
        }
    }
}
