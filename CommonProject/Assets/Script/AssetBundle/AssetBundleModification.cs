

#if UNITY_EDITOR


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;
using System.Text;

namespace sdy.AssetBundleManager
{


    /// <summary>
    /// 批量修改AssetBundle的Name与Variant的编辑器窗口
    /// </summary>
    public class AssetBundleModification : EditorWindow
    {

        [MenuItem("AssetsManager/批量修改AssetBundle")]
        static void AddWindow()
        {
            //创建窗口
            AssetBundleModification window = (AssetBundleModification)EditorWindow.GetWindow
                (typeof(AssetBundleModification), false, "批量修改AssetBundle");
            window.Show();

        }

        //输入文字的内容
        private string Path = "Assets/AssetBundles/", AssetBundleName = "", Variant = "", Version = "";
        private bool IsThisName = true;
        private bool IsThisVersion = true;

        void OnGUI()
        {
            GUIStyle text_style = new GUIStyle();
            text_style.fontSize = 15;
            text_style.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("默认使用源文件名", GUILayout.MinWidth(120));
            IsThisName = EditorGUILayout.Toggle(IsThisName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("AssetBundleName:", GUILayout.MinWidth(120));
            if (IsThisName)
                GUILayout.Label("源文件名.unity3d", GUILayout.MinWidth(120));
            else
                AssetBundleName = EditorGUILayout.TextField(AssetBundleName.ToLower());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Variant:", GUILayout.MinWidth(120));
            Variant = EditorGUILayout.TextField(Variant.ToLower());
            EditorGUILayout.EndHorizontal();

            GUILayout.Label("\n");

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("文件夹路径", GUILayout.MinWidth(60));
            if (GUILayout.Button("浏览", GUILayout.MinWidth(60))) { OpenFolder(); }
            Path = EditorGUILayout.TextField(Path);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("修改该文件夹下的AssetName及Variant")) { SetSettings(); }
            if (GUILayout.Button("清除所有未被引用的AssetName及Variant"))
            {
                AssetDatabase.RemoveUnusedAssetBundleNames();
            }
            if (GUILayout.Button("清空所有AssetName及Variant"))
            {
                ClearAssetBundlesName();
            }

            //EditorGUILayout.BeginHorizontal();
            GUILayout.Label("使用默认版本号", GUILayout.MinWidth(120));
            IsThisVersion = EditorGUILayout.Toggle(IsThisVersion);
            //EditorGUILayout.EndHorizontal();

            //EditorGUILayout.BeginHorizontal();
            GUILayout.Label("版本号:", GUILayout.MinWidth(120));
            if (IsThisVersion)
                GUILayout.Label("1.0", GUILayout.MinWidth(120));
            else
                Version = EditorGUILayout.TextField(Version.ToLower());
            //EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("生成配置文件"))
            {
                BuildVersionTxt();
            }
        }

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
                Path = m_path.Substring(firstindex) + "/";
                EditorUtility.FocusProjectWindow();
            }
        }

        /// <summary>
        /// 此函数用来修改AssetBundleName与Variant
        /// </summary>
        void SetSettings()
        {
            if (Directory.Exists(Path))
            {
                DirectoryInfo direction = new DirectoryInfo(Path);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);


                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }
                    AssetImporter ai = AssetImporter.GetAtPath(files[i].FullName.Substring(files[i].FullName.IndexOf("Assets")));
                    if (IsThisName)
                        ai.SetAssetBundleNameAndVariant(files[i].Name.Replace(".", "_") + ".unity3d", Variant);
                    else
                        ai.SetAssetBundleNameAndVariant(AssetBundleName, Variant);
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

        void BuildVersionTxt()
        {
            BuildVersion(Application.dataPath + "/AssetBundles/Test");
        }

        static void BuildVersion(string path)
        {
            // 获取Res文件夹下所有文件的相对路径和MD5值  
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            StringBuilder versions = new StringBuilder();
            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files[i];
                string extension = filePath.Substring(files[i].LastIndexOf("."));
                if (extension != ".meta")
                {
                    string relativePath = filePath.Replace(path, "").Replace("\\", "/");
                    string md5 = CalculateMD5(filePath);
                    versions.Append(relativePath).Append(",").Append(md5).Append("\n");
                }
            }
            // 生成配置文件  
            string vpath = Application.dataPath + "/AssetBundles/version.txt";
            FileStream stream = new FileStream(vpath, FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
        }



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



            void OnInspectorUpdate()
        {
            this.Repaint();//窗口的重绘
        }
    }

}




#endif
