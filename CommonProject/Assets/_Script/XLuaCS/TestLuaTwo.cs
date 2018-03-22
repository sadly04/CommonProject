using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using XLua;
using sdy.AssetBundleManager;

namespace sdy.Lua
{


    [Hotfix]
    public class TestLuaTwo : MonoBehaviour
    {
        LuaEnv luaenv = new LuaEnv();
        public int num = 0;
        public string textvalue = null;

        // Use this for initialization
        IEnumerator Start()
        {
            yield return null;

            AssetBundleCreateRequest abcr = AssetBundleLoad.LoadAssetFromStreamingAssetsAsync("common");
            yield return abcr;
            AssetBundle common = abcr.assetBundle;

            AssetBundleRequest abr = common.LoadAssetAsync("hotfix.lua");
            yield return abr;
            TextAsset text = abr.asset as TextAsset;
            textvalue = text.text;

            common.Unload(true);

            //AssetBundleCreateRequest abcr1 = AssetBundle.LoadFromFileAsync(
            //    Application.persistentDataPath + "common");

            string rwp = Application.persistentDataPath + "/common.txt";
            //Debug.Log(rwp);
            //Debug.Log(Application.dataPath);

            //if (!File.Exists(rwp))
            //{
            //    //File.Create(rwp + ".txt");
            //    File.WriteAllText(rwp + ".txt", text.text);
            //}

            if(File.Exists(rwp))
            {
                Debug.Log("文件存在");
                File.Delete(rwp);
            }


            if (File.Exists(rwp))
            {
                Debug.Log("删除失败");
            }
            else
            {
                Debug.Log("删除成功");
            }


            //if (!File.Exists(Application.persistentDataPath + "/common"))
            //{
            //    Debug.Log("存在ab包");
            //    File.Copy("G:/CommonProject/CommonProject/Assets/StreamingAssets/AssetBundles/Windows/common",
            //        Application.persistentDataPath + "/common");
            //}

            if (!Directory.Exists(Application.persistentDataPath + "/AssetBundles"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/AssetBundles");
            }

            if (!File.Exists(Application.persistentDataPath + "/common"))
            {
                WWW www = new WWW("G:/CommonProject/CommonProject/Assets/StreamingAssets/AssetBundles/Windows/common");
                yield return www;
                if (string.IsNullOrEmpty(www.error))
                {
                    FileStream stream = File.Create(Application.persistentDataPath + "/common");
                    stream.Write(www.bytes, 0, www.bytes.Length);
                    stream.Flush();
                    stream.Close();
                }
            }

            AssetBundleCreateRequest abcr1 = AssetBundle.LoadFromFileAsync(Application.persistentDataPath + "/common");
            yield return abcr1;

            AssetBundle common1 = abcr1.assetBundle;

            AssetBundleRequest abr1 = common1.LoadAssetAsync("hotfix.lua");
            yield return abr1;

            TextAsset txt = abr1.asset as TextAsset;
            Debug.Log(txt.text);

            //Debug.Log(File.ReadAllText(rwp + ".txt"));

            //StreamReader read = new StreamReader(rwp + ".txt");
            //string s = read.ReadToEnd();
            //Debug.Log(s);



            //luaenv.DoString(@"");

            //luaenv.DoString(@"  
            //    xlua.hotfix(CS.sdy.Lua.TestLuaTwo, 'Add', function(self, a, b)  
            //      return a + b  
            //    end)  
            //");

            //luaenv.DoString(@"require 'hotfix'");

            //TestTableValue table = luaenv.Global.Get<TestTableValue>("tablevalue");

            //Debug.Log(table.name + table.age + table.description);

            //int num = Add(2, 1);
            //print(num);
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


                //luaenv.DoString(@"require 'hotfix'");

                //luaenv.DoString(@"  
                //    xlua.hotfix(CS.sdy.Lua.TestLuaTwo, 'Add', function(self, a, b)  
                //      return a + b  
                //    end)  
                //");


                num = Add(2, 1);
                print(num);

                

                //luaenv.Dispose();
            }

            GUI.TextField(new Rect(310, 100, 300, 150), num.ToString());
        }



        void OnDestroy()
        {
            //luaenv.Dispose();
        }
    }


    public class TestTableValue
    {
        public string name;
        public string age;
        public string description;
    }

}
