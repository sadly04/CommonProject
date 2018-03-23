
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEditor;
using System.IO;



namespace sdy.AssetBundleManager
{

    /// <summary>
    /// AssetBundle打包编辑器窗口
    /// </summary>
    public class AssetBundlePack : EditorWindow
    {

        [MenuItem("AssetsManager/AssetBundle打包")]
        static void OpenWindow()
        {
            AssetBundlePack window = (AssetBundlePack)EditorWindow.GetWindow
                (typeof(AssetBundlePack), false, "AssetBundle打包");
            window.Show();
        }

        private string Path = null;


        private void OnGUI()
        {
            GUIStyle text_style = new GUIStyle();
            text_style.fontSize = 15;
            text_style.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("文件夹路径:", GUILayout.MinWidth(60));
            if (GUILayout.Button("浏览", GUILayout.MinWidth(60))) { OpenFolder(); }
            Path = EditorGUILayout.TextField(Path);
            EditorGUILayout.EndHorizontal();

            GUILayout.Label("如果没有指定文件夹位置，则生成的AB包在StreamingAssets/AssetBundles下");

            if (GUILayout.Button("Pack(Android)")) { BuildAssetBundleForAndroid(); };
            if (GUILayout.Button("Pack(IOS)")) { BuildAssetBundleForIOS(); };
            if (GUILayout.Button("Pack(Windows)")) { BuildAssetBundleForWindows(); };
            if (GUILayout.Button("Pack(All)"))
            {
                BuildAssetBundleForWindows();
                BuildAssetBundleForAndroid();
                BuildAssetBundleForAndroid();
            };
        }

        void BuildAssetBundleForWindows()
        {
            BuildAssetBundle(Path, AssetBundleLoad.WindowsDirectory, BuildTarget.StandaloneWindows64);
        }

        void BuildAssetBundleForIOS()
        {
            BuildAssetBundle(Path, AssetBundleLoad.IOSDirectory, BuildTarget.iOS);
        }

        void BuildAssetBundleForAndroid()
        {
            BuildAssetBundle(Path, AssetBundleLoad.AndroidDirectory, BuildTarget.Android);
        }


        void BuildAssetBundle(string folderPath, string defaultPath, BuildTarget targetType)
        {
            string path;
            if (folderPath.Length != 0)
            {
                path = folderPath;
            }
            else
            {
                if (!Directory.Exists(AssetBundleLoad.StreamingAssetDirectory))
                {
                    Directory.CreateDirectory(AssetBundleLoad.StreamingAssetDirectory);
                }

                if (!Directory.Exists(AssetBundleLoad.StreamingAssetDirectory
                    + AssetBundleLoad.AssetBundleDirectory))
                {
                    Directory.CreateDirectory(AssetBundleLoad.StreamingAssetDirectory
                        + AssetBundleLoad.AssetBundleDirectory);
                }

                if (!Directory.Exists(AssetBundleLoad.StreamingAssetDirectory
                    + AssetBundleLoad.AssetBundleDirectory + defaultPath))
                {
                    Directory.CreateDirectory(AssetBundleLoad.StreamingAssetDirectory
                        + AssetBundleLoad.AssetBundleDirectory + defaultPath);
                }

                path = AssetBundleLoad.StreamingAssetDirectory 
                    + AssetBundleLoad.AssetBundleDirectory + defaultPath;
            }

            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, targetType);
        }


        /// <summary>
        /// 此函数用来打开文件夹修改路径
        /// </summary>
        void OpenFolder()
        {
            string m_path = EditorUtility.OpenFolderPanel("选择文件夹", "", "");
            if (m_path.Length != 0 && !m_path.Contains(Application.dataPath))
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


        void OnInspectorUpdate()
        {
            this.Repaint();//窗口的重绘
        }
    }

}


#endif