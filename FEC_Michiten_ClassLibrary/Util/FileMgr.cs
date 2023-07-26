using System;
using System.IO;

namespace FEC_Michiten_ClassLibrary.Util
{
    public static class FileMgr
    {
        public static string _lastErr = string.Empty;


        /// <summary>
        /// 指定パスに新規ファイルを作成する
        /// ※引数で隠しファイル指定可
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="text"></param>
        /// <param name="hidden"></param>
        /// <returns></returns>
        public static bool CreateTextFile(string fullPath, string text, bool hidden = false)
        {
            try
            {
                // フォルダがない場合は作成
                var folderPath = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                File.WriteAllText(fullPath, text);

                // 隠し指定がある場合は属性変更
                if (hidden)
                {
                    FileAttributes fa = File.GetAttributes(fullPath);
                    fa = fa | FileAttributes.Hidden;
                    File.SetAttributes(fullPath, fa);
                }
            }
            catch (Exception ex)
            {
                _lastErr = ex.Message;
                return false;
            }

            return true;
        }


        /// <summary>
        /// 指定ファイルの内容をReadして返す
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string ReadTextFile(string fullPath)
        {
            string allText = null;

            try
            {
                allText = File.ReadAllText(fullPath);
            }
            catch (Exception ex)
            {
                _lastErr = ex.Message;
                return allText;
            }
            return allText;
        }


        public static string GetLastErrMsg()
        {
            return _lastErr;
        }



    }
}
