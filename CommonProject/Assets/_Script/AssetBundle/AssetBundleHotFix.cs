using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;


namespace sdy.AssetBundleManager
{

    public class AssetBundleHotFix : MonoBehaviour
    {


        public static void BuildVersion(string path)
        {
            // 获取Res文件夹下所有文件的相对路径和MD5值  
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            StringBuilder versions = new StringBuilder();
            for (int i = 0; i < files.Length; i++)
            {
                Debug.Log("第" + i + "个:" + files[i]);
                string filePath = files[i];
                string extension = filePath.Substring(files[i].LastIndexOf("."));
                if (extension == ".unity3d")
                {
                    string relativePath = filePath.Replace(path, "").Replace("\\", "/");
                    string md5 = CalculateMD5(filePath);
                    versions.Append(relativePath).Append(",").Append(md5).Append("\n");
                }
            }
            // 生成配置文件  
            FileStream stream = new FileStream(path + "version.txt", FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
        }



        public static string CalculateMD5(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                System.Security.Cryptography.MD5 md5 =
                    new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("md5file() fail, error:" + ex.Message);
            }
        }
    }


}