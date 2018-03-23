#if UNITY_EDITOR


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;
using System.Text;

namespace sdy.HotFix
{

    public class EditorHotFix : EditorWindow
    {

        [MenuItem("HotFix/批量修改资源")]
        static void OpenWindow()
        {
            EditorHotFix window = (EditorHotFix)EditorWindow.GetWindow(typeof(EditorHotFix), false, "批量修改资源");
            window.Show();
        }


        private bool isDefaultName;
        private string assetBundleName = "";
        private string variantName = "";
        private string folderPath = "";


        private void OnGUI()
        {
            GUIStyle text_style = new GUIStyle();
            text_style.fontSize = 15;
            text_style.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("默认使用源文件夹名", GUILayout.MinWidth(120));
            isDefaultName = EditorGUILayout.Toggle(isDefaultName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("AssetBundleName:", GUILayout.MinWidth(120));
            if (isDefaultName)
            {
                GUILayout.Label("源文件夹名", GUILayout.MinWidth(120));
            }
            else
            {
                assetBundleName = EditorGUILayout.TextField(assetBundleName.ToLower());
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Variant:", GUILayout.MinWidth(120));
            variantName = EditorGUILayout.TextField(variantName.ToLower());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("选择文件夹路径", GUILayout.MinWidth(60));
            if (GUILayout.Button("浏览", GUILayout.MinWidth((60)))) { OpenFolder(); }
            folderPath = EditorGUILayout.TextField(folderPath);
            EditorGUILayout.EndHorizontal();

            GUILayout.Label("\n");

            if (GUILayout.Button("修改该文件夹下的AssetName和Variant")) { SetSettings(); }
            if (GUILayout.Button("清除所有未被引用的AssetName和Variant"))
            { AssetDatabase.RemoveUnusedAssetBundleNames(); }
            if (GUILayout.Button("清空所有的AssetName和Variant")) { ClearAssetBundlesName(); }
            if (GUILayout.Button("在该文件夹下生成AssetBundle包(Android)")) { BuildAssetBundleForAndroid(); }
            if (GUILayout.Button("在该文件夹下生成AssetBundle包(IOS)")) { BuildAssetBundleForIOS(); }
            if (GUILayout.Button("在该文件夹下生成AssetBundle包(Window)")) { BuildAssetBundleForWindows(); }

            if (GUILayout.Button("在该文件夹下生成AB包的配置文件")) { BuildVersion(); }
        }


        private void OnInspectorUpdate()
        {
            this.Repaint();//窗口的重绘
        }


        #region AssetBundle Function


        void BuildAssetBundleForWindows()
        {
            string path = folderPath;
            BuildPipeline.BuildAssetBundles(path,
                BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }

        void BuildAssetBundleForIOS()
        {
            string path = folderPath;
            BuildPipeline.BuildAssetBundles(path,
                BuildAssetBundleOptions.None, BuildTarget.iOS);
        }

        void BuildAssetBundleForAndroid()
        {
            string path = folderPath;
            BuildPipeline.BuildAssetBundles(path,
                BuildAssetBundleOptions.None, BuildTarget.Android);
        }


        /// <summary>
        /// 此函数用来修改AssetBundleName与Variant
        /// </summary>
        void SetSettings()
        {
            if (Directory.Exists(folderPath))
            {
                DirectoryInfo direction = new DirectoryInfo(folderPath);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);


                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }
                    AssetImporter ai = AssetImporter.GetAtPath(files[i].FullName.Substring(files[i].FullName.IndexOf("Assets")));
                    if (isDefaultName)
                        ai.SetAssetBundleNameAndVariant(files[i].Name.Replace(".", "_") + ".unity3d", variantName);
                    else
                        ai.SetAssetBundleNameAndVariant(assetBundleName, variantName);
                }
                AssetDatabase.Refresh();
            }
        }


        /// <summary>
        /// 清除之前设置过的AssetBundleName，避免产生不必要的资源也打包
        /// 工程中只要设置了AssetBundleName的，都会进行打包
        /// </summary>
        static void ClearAssetBundlesName()
        {
            int length = AssetDatabase.GetAllAssetBundleNames().Length;
            string[] oldAssetBundleNames = new string[length];
            for (int i = 0; i < length; i++)
            {
                oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
            }

            for (int j = 0; j < oldAssetBundleNames.Length; j++)
            {
                AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
            }
        }


        #endregion


        /// <summary>
        /// 此函数用来打开文件夹修改路径
        /// </summary>
        void OpenFolder()
        {
            string m_path = EditorUtility.OpenFolderPanel("选择文件夹", "", "");
            if (!m_path.Contains(Application.dataPath))
            {
                Debug.LogError("路径应在当前工程目录下");
                return;
            }
            if (m_path.Length != 0)
            {
                int firstindex = m_path.IndexOf("Assets");
                folderPath = m_path.Substring(firstindex) + "/";
                EditorUtility.FocusProjectWindow();
            }
        }



        void BuildVersion()
        {
            string path = Application.dataPath + "/StreamingAssets/AssetBundles/";

            // 获取文件夹下所有文件的相对路径和MD5值  
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            StringBuilder versions = new StringBuilder();
            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files[i];
                string extension = null;
                if (filePath.Contains("."))
                {
                    extension = filePath.Substring(files[i].LastIndexOf("."));
                }
                if (extension != ".meta")
                {
                    string relativePath = filePath.Replace(path, "").Replace("\\", "/");
                    string md5 = CalculateMD5(filePath);
                    versions.Append(relativePath).Append(",").Append(md5).Append("\n");
                }
            }
            // 生成配置文件  
            string vpath = folderPath + "version.txt";
            FileStream stream = new FileStream(vpath, FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
        }



        /// <summary>
        /// 计算路径下文件的md5值
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        static string CalculateMD5(string filePath)
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


#endif