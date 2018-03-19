using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;


namespace sdy.AssetBundleManager
{


    public class AssetBundleLoad : MonoBehaviour
    {

        public static string StreamingAssetDirectory = "Assets/StreamingAssets/";
        public static string AssetBundleDirectory = "AssetBundles/";
        public static string WindowsDirectory = "Windows";
        public static string AndroidDirectory = "Android";
        public static string IOSDirectory = "IOS";


        public byte[] b;

        public static AssetBundleCreateRequest LoadAssetFromStreamingAssetsAsync(string bundleLocalPath)
        {

            string fullPath = "";

#if !UNITY_EDITOR && UNITY_ANDROID

            fullPath = "jar:file://" + Application.dataPath + "!/assets/" + AssetBundleDirectory
            + AndroidDirectory + "/" + bundleLocalPath;

#elif !UNITY_EDITOR && UNITY_IOS

            fullPath = Application.dataPath + "/Raw/" + AssetBundleDirectory
            + iOSDirectory + "/" + bundleLocalPath;

#elif UNITY_EDITOR

            fullPath = Application.streamingAssetsPath + "/" + AssetBundleDirectory + "/" +
                 WindowsDirectory + "/" + bundleLocalPath;

#endif
            Debug.Log(fullPath);

            return AssetBundle.LoadFromFileAsync(fullPath);

        }


        public static AssetBundle LoadAssetFromStreamingAssets(string bundleLocalPath)
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
            return AssetBundle.LoadFromFile(fullPath);

        }

        /// <summary>
        /// 通过stream读取assetbundle
        /// </summary>
        /// <param name="bundleLocalPath"></param>
        /// <returns></returns>
//        public static AssetBundleCreateRequest LoadAssetFromStreamingAssetsStaeamAsync(string bundleLocalPath)
//        {

//            string fullPath = "";

//#if !UNITY_EDITOR && UNITY_ANDROID

//            fullPath = "jar:file://" + Application.dataPath + "!/assets/" + AndroidDirectory + "/" + bundleLocalPath;

//#elif !UNITY_EDITOR && UNITY_IOS

//            fullPath = Application.dataPath + "/Raw/" + iOSDirectory + "/" + bundleLocalPath;

//#elif UNITY_EDITOR

//            fullPath = Application.streamingAssetsPath + "/" + AssetBundleDirectory +
//                 WindowsDirectory + bundleLocalPath;

//#endif

//            StreamReader sr = new StreamReader(fullPath);
//            return AssetBundle.LoadFromStreamAsync(sr.BaseStream);
//        }


        public IEnumerator LoadAssetBundleFormServer()
        {
            WWW www = new WWW("http://420099788@qq.com");
            yield return www;

            if (www.error == null)
            {
                b = www.bytes;
                Debug.Log(www.bytes);
            }
        }
    }

}
