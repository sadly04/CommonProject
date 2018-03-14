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
        Texture Tex2;

        // Use this for initialization
        IEnumerator Start()
        {
            AssetBundleCreateRequest abrq = AssetBundleLoad.LoadAssetFromStreamingAssetsAsync("/main");
            yield return abrq;
            MainAB = abrq.assetBundle;

            AssetBundleRequest abq = MainAB.LoadAssetAsync("bianbian");
            yield return abq;
            Tex1 = abq.asset as Texture;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
