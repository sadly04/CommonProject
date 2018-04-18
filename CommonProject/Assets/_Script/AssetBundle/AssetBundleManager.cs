using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using System;
using Newtonsoft.Json;

namespace sdy.AssetBundleManager
{

    public class AssetBundleManager : MonoBehaviour
    {
        [SerializeField]
        AssetBundle MainAB;

        public Texture Tex1;
        public Texture Tex2;

        public Dictionary<string, string> ManifestDC;

        // Use this for initialization
        IEnumerator Start()
        {
            AssetBundleCreateRequest abrq = AssetBundleLoad.LoadAssetFromStreamingAssetsAsync("/main");
            //AssetBundleCreateRequest abrq = AssetBundleLoad.LoadAssetFromStreamingAssetsStaeamAsync("/main");
            yield return abrq;
            MainAB = abrq.assetBundle;
            MainAB.Unload(true);


            string url = Application.streamingAssetsPath + "/AssetBundles/Windows/Windows";
            WWW www = WWW.LoadFromCacheOrDownload(url, 0);
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("Is null");
            }
            else
            {
                AssetBundleManifest am = www.assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                if (am == null)
                {
                    Debug.Log("am is null");
                }
                else
                {
                    string[] dp = am.GetAllAssetBundles();
                    foreach (var v in dp)
                    {
                        Debug.Log(v);
                    }
                }
            }

            //Debug.Log(manifest.GetHashCode());
            //string[] dps = manifest.GetDirectDependencies();

            //foreach (var st in dps)
            //{
            //    Debug.Log(st);
            //}

            /*
            AssetBundleRequest abq = MainAB.LoadAssetAsync("1");
            yield return abq;
            Tex1 = abq.asset as Texture;


            AssetBundleRequest abq1 = MainAB.LoadAssetAsync("2");
            yield return abq1;
            Tex2 = abq1.asset as Texture;
            */
            //StartCoroutine(GetComponent<AssetBundleLoad>().LoadAssetBundleFormServer());


            string path = Application.dataPath;
            Debug.Log(path);
            string spath = Application.streamingAssetsPath;
            Debug.Log(spath);
            string ppath = Application.persistentDataPath;
            Debug.Log(ppath);

            string filepath = "/AssetBundles/Windows/main.manifest";
            if (File.Exists(spath + filepath))
            {
                string str = File.ReadAllText(spath + filepath);
                Debug.Log(str);
            }

            

        }

    }


    public class ManifestData
    {

    }
}
