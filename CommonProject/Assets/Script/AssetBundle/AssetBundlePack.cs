
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

        private void OnGUI()
        {
            //GUIStyle text_style = new GUIStyle();
            //text_style.fontSize = 15;
            //text_style.alignment = TextAnchor.MiddleCenter;

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
            string path = AssetBundleLoad.StreamingAssetDirectory 
                + AssetBundleLoad.AssetBundleDirectory
                + AssetBundleLoad.WindowsDirectory;
            BuildPipeline.BuildAssetBundles(path, 
                BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }

        void BuildAssetBundleForIOS()
        {
            string path = AssetBundleLoad.StreamingAssetDirectory
                + AssetBundleLoad.AssetBundleDirectory
                + AssetBundleLoad.IOSDirectory;
            BuildPipeline.BuildAssetBundles(path,
                BuildAssetBundleOptions.None, BuildTarget.iOS);
        }

        void BuildAssetBundleForAndroid()
        {
            string path = AssetBundleLoad.StreamingAssetDirectory
                + AssetBundleLoad.AssetBundleDirectory
                + AssetBundleLoad.AndroidDirectory;
            BuildPipeline.BuildAssetBundles(path,
                BuildAssetBundleOptions.None, BuildTarget.Android);
        }


        void OnInspectorUpdate()
        {
            this.Repaint();//窗口的重绘
        }
    }

}


#endif