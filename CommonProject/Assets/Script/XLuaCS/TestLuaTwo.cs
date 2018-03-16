using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XLua;
using sdy.AssetBundleManager;

namespace sdy.Lua
{


    [Hotfix]
    public class TestLuaTwo : MonoBehaviour
    {
        LuaEnv luaenv = new LuaEnv();

        // Use this for initialization
        IEnumerator Start()
        {
            AssetBundleCreateRequest abcr = AssetBundleLoad.LoadAssetFromStreamingAssetsAsync("/common");
            yield return abcr;
            AssetBundle common = abcr.assetBundle;

            AssetBundleRequest abr = common.LoadAssetAsync("hotfix.lua");
            yield return abr;
            TextAsset text = abr.asset as TextAsset;

            luaenv.DoString(text.text);

            //luaenv.DoString(@"  
            //    xlua.hotfix(CS.sdy.Lua.TestLuaTwo, 'Add', function(self, a, b)  
            //      return a + b  
            //    end)  
            //");


            int num = Add(2, 1);
            print(num);
        }

        // Update is called once per frame
        void Update()
        {
            if (luaenv != null)
            {
                luaenv.Tick();
            }
        }


        public int Add(int a, int b)
        {
            return a - b;
        }


        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 100, 300, 150), "hotfix"))
            {
                //byte[] bytes = System.IO.File.ReadAllBytes(Application.dataPath + "/Resources/hotfix.lua");

                //Debug.Log(Application.dataPath);


                luaenv.DoString(@"require 'hotfix'");

                //luaenv.DoString(@"  
                //    xlua.hotfix(CS.sdy.Lua.TestLuaTwo, 'Add', function(self, a, b)  
                //      return a + b  
                //    end)  
                //");


                int num = Add(2, 1);
                print(num);

                //luaenv.Dispose();
            }
        }



        void OnDestroy()
        {
            //luaenv.Dispose();
        }
    }

}
