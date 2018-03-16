using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XLua;

namespace sdy.Lua
{
    [Hotfix]
    public class TestLuaThree : MonoBehaviour
    {
        LuaEnv luaenv = new LuaEnv();

        // Use this for initialization
        void Start()
        {
            luaenv.DoString("require 'change'");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }


}