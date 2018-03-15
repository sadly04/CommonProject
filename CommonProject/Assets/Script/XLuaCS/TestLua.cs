using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XLua;

namespace sdy.Lua
{
    [Hotfix]
    public class TestLua : MonoBehaviour
    {
        LuaEnv luaenv = new LuaEnv();

        // Use this for initialization  
        void Start()
        {
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

        void OnGUI()
        {
            if (GUI.Button(new Rect(10, 100, 300, 150), "Hotfix"))
            {
                luaenv.DoString("require 'hotfix'");

                //luaenv.DoString(@"  
                //    xlua.hotfix(CS.test, 'Add', function(self, a, b)  
                //      return a + b  
                //  end)  
                //");  

                int num = Add(2, 1);
                print(num);
            }
        }

        void OnDestroy()
        {
            luaenv.Dispose();
        }
    }


}