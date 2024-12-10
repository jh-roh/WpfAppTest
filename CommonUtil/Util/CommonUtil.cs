using CommonUtil;
using System.IO;
using System;

namespace CommonUtil.Util
{
    public class CommonUtil
    {
        public static string GetBaseDirectory(string customDirectory)
        {
            var baseDir = Path.GetPathRoot(customDirectory);

            if (baseDir.StartsWith(@"\") == false)
                baseDir = @"C:\";

            return Path.Combine(baseDir, customDirectory);
        }

        public static object GetPropertyValue(object source, string propertyName)
        {
            object result = null;
            var temp = source.GetType().GetProperty(propertyName);

            if (temp != null)
                result = temp.GetValue(source, null);

            return result;
        }

        public static void SetPropertyValue(object source, string propertyName, object value)
        {
            var temp = source.GetType().GetProperty(propertyName);

            if (temp != null)
                temp.SetValue(source, value, null);
        }

        public static string GetUniqueFileName(string path, string fileName)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fullPath = Path.Combine(path, fileName);

                if (!File.Exists(fullPath))
                    return fullPath;

                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                var extension = Path.GetExtension(fileName);

                for (int i = 2; ; i++)
                {
                    var newFullPath = Path.Combine(path, string.Format("{0}_{1}{2}", fileNameWithoutExtension, i, extension));
                    if (!File.Exists(newFullPath))
                        return newFullPath;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Path.Combine(path, fileName);
            }
        }

        /// <summary>
        /// 사진 가져오기
        /// </summary>
        /// <param name="path">사진 경로</param>
        /// <returns>JPG의 byte배열</returns>
        public static byte[] GetPicture(string path)
        {
            try
            {
                if (File.Exists(path) == false)
                    return null;

                byte[] bytes = null;
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    file.Close();
                }
                return bytes;
            }
            catch (Exception/* ex*/)
            {
                return null;
            }
        }

    }

}


