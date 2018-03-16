using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;

namespace sdy.AssetBundleManager
{

    public class AssetBundleManager : MonoBehaviour
    {
        [SerializeField]
        AssetBundle MainAB;

        public Texture Tex1;
        public Texture Tex2;

        // Use this for initialization
        IEnumerator Start()
        {
            AssetBundleCreateRequest abrq = AssetBundleLoad.LoadAssetFromStreamingAssetsAsync("/main");
            //AssetBundleCreateRequest abrq = AssetBundleLoad.LoadAssetFromStreamingAssetsStaeamAsync("/main");
            yield return abrq;
            MainAB = abrq.assetBundle;

            AssetBundleRequest abq = MainAB.LoadAssetAsync("bianbian");
            yield return abq;
            Tex1 = abq.asset as Texture;


            AssetBundleRequest abq1 = MainAB.LoadAssetAsync("cangying");
            yield return abq1;
            Tex2 = abq1.asset as Texture;
            //StartCoroutine(GetComponent<AssetBundleLoad>().LoadAssetBundleFormServer());
        }

    }

}
