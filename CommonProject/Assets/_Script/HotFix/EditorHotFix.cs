#if UNITY_EDITOR


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;

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
        private string versionSavePath = "";


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

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("选择配置文件存储路径", GUILayout.MinWidth(60));
            if (GUILayout.Button("浏览", GUILayout.MinWidth((60)))) { OpenSavePathFolder(); }
            versionSavePath = EditorGUILayout.TextField(versionSavePath);
            EditorGUILayout.EndHorizontal();

            GUILayout.Label("\n");

            if (GUILayout.Button("修改该文件夹下的AssetName和Variant")) { SetSettings(); }
            if (GUILayout.Button("清除所有未被引用的AssetName和Variant"))
            { AssetDatabase.RemoveUnusedAssetBundleNames(); }
            if (GUILayout.Button("清空所有的AssetName和Variant")) { ClearAssetBundlesName(); }
            if (GUILayout.Button("在该文件夹下生成AssetBundle包(Android)")) { BuildAssetBundleForAndroid(); }
            if (GUILayout.Button("在该文件夹下生成AssetBundle包(IOS)")) { BuildAssetBundleForIOS(); }
            if (GUILayout.Button("在该文件夹下生成AssetBundle包(Window)")) { BuildAssetBundleForWindows(); }
            //if (GUILayout.Button("在该文件夹下生成AssetBundle包(全平台)"))
            //{
            //    BuildAssetBundleForAndroid();
            //    BuildAssetBundleForIOS();
            //    BuildAssetBundleForWindows();
            //}
            if (GUILayout.Button("在该文件夹下生成AB包的配置文件")) { }
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


        /// <summary>
        /// 此函数用来打开文件夹修改路径
        /// </summary>
        void OpenSavePathFolder()
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
                versionSavePath = m_path.Substring(firstindex) + "/";
                EditorUtility.FocusProjectWindow();
            }
        }
    }

}


#endif