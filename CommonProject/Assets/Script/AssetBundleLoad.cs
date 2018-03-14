using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace sdy.AssetBundleManager
{


    public class AssetBundleLoad : MonoBehaviour
    {

        public static string StreamingAssetDirectory = "Assets/StreamingAssets/";
        public static string AssetBundleDirectory = "AssetBundles";
        public static string WindowsDirectory = "/Windows";
        public static string AndroidDirectory = "/Android";
        public static string IOSDirectory = "/IOS";


        public static AssetBundleCreateRequest LoadAssetFromStreamingAssetsAsync(string bundleLocalPath)
        {

            string fullPath = "";

#if !UNITY_EDITOR && UNITY_ANDROID

            fullPath = "jar:file://" + Application.dataPath + "!/assets/" + AndroidDirectory + "/" + bundleLocalPath;

#elif !UNITY_EDITOR && UNITY_IOS

            fullPath = Application.dataPath + "/Raw/" + iOSDirectory + "/" + bundleLocalPath;

#elif UNITY_EDITOR

            fullPath = System.IO.Path.Combine(Application.streamingAssetsPath,
                System.IO.Path.Combine(WindowsDirectory, bundleLocalPath));

#endif
            return AssetBundle.LoadFromFileAsync(fullPath);

        }

    }

}
