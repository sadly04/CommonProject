using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XLua;

namespace sdy.Lua
{
    public class TestLua : MonoBehaviour
    {
        private void Start()
        {
            LuaEnv luaenv = new LuaEnv();
            luaenv.DoString("CS.UnityEngine.Debug.Log('Hello World')");
            luaenv.Dispose();
        }
    }


}