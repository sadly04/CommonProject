using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XLua;
using System;

namespace sdy.Lua
{
    [Hotfix]
    public class TestLuaThree : MonoBehaviour
    {
        LuaEnv luaenv = new LuaEnv();

        string script = @"

--csharp 调用 xlua
a = 1
b = 'hello world'
c = true

d = {
     f1 = 1, f2 = 2,
     add = function(self, a, b)
         print('输出相加值')
         return a + b
     end
}

function e()
     print('输出委托')
end

function er()
     print('返回委托')
     return e
end


--xlua 调用 csharp

local go = CS.UnityEngine.GameObject('new gameobject')
local time = CS.UnityEngine.Time.deltaTime
print(time)

local self = CS.UnityEngine.GameObject.Find('[ScriptController]'):GetComponent('TestLuaThree')
self:LogSomething()

function lognothing()
     print('there is nothing')
end

--协程下使用 使用本地变量记录cs的yield_return
--在if的结束后必须加上end

local util = require 'xlua.util'
local yield_return = (require 'cs_coroutine').yield_return

local co = coroutine.create(function()
     print('start coroutine')
     yield_return(CS.UnityEngine.WaitForSeconds(3))
     print('has wait for 3 seconds')

     local www = CS.UnityEngine.WWW('http://www.qq.com')
     yield_return(www)
     if not www.error then
          print(www.bytes)
     else
          print('error:', www.error)
     end
     lognothing()
end)
assert(coroutine.resume(co))


--csharp 调用 xlua 方法 常用的开始 更新 销毁


function start()
     print('in start')
end

function update()
     print('in update')
end

function destory()
     print('has destory')
end


function reflact()
     CS.UnityEngine.GameObject.Find('Cube'):GetComponent('Transform').localScale = CS.UnityEngine.Vector3(5, 5, 5)
end


--异步模拟

function async_charge(num, cb) --模拟的异步充值
     print('requst setver...')
     cb(true, num)
end

local recharge = util.async_to_sync(async_charge)

function async_test()
     local r1, r2 = recharge(10)
     print('recharge result : ', r1, r2)
end


--热修复
function sync_hotfix()
     xlua.hotfix(CS.sdy.Lua.TestLuaThree, 'Update', function(self)
          print('热修复输出')
     end)
end

        ";

        [CSharpCallLua]
        public interface Facein
        {
            int f1 { get; set; }
            int f2 { get; set; }
            int add(int a, int b);
        }

        [CSharpCallLua]
        public delegate Action ACdele();

        [CSharpCallLua]
        public delegate void Dele();


        public Action luaupdate;
        public Action syncfix;

        // Use this for initialization
        void Start()
        {

            luaenv.DoString(script);

            luaenv.Global.Get("update", out luaupdate);

            Action a;
            luaenv.Global.Get("reflact", out a);
            a();
            a = null;

            Action b;
            luaenv.Global.Get("async_test", out b);
            b();
            b = null;

            luaenv.Global.Get("sync_hotfix", out syncfix);

            /*
            Debug.Log(luaenv.Global.Get<int>("a"));
            Debug.Log(luaenv.Global.Get<string>("b"));
            Debug.Log(luaenv.Global.Get<bool>("c"));

            Action e = luaenv.Global.Get<Action>("e");
            e();

            Dele a = luaenv.Global.Get<Dele>("e");
            a();

            ACdele ae = luaenv.Global.Get<ACdele>("er");
            e = ae();
            e();
            */

            //Facein inf = luaenv.Global.Get<Facein>("d");
            //Debug.Log(inf.f1);
            //需要添加到代码生成器中，否则出错

        }

        // Update is called once per frame
        void Update()
        {
            if (luaupdate != null)
            {
                luaupdate();
            }

            if (luaenv != null)
            {
                luaenv.Tick();
            }
        }


        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 300, 80), "Hotfix"))
            {
                //luaenv.DoString(@"
                //xlua.hotfix(CS.sdy.Lua.TestLuaThree, 'Update', function(self)
                //     print('热修复输出')
                //end)
                //");

                if (syncfix != null)
                {
                    syncfix();
                }
            }
        }


        public void LogSomething()
        {
            Debug.Log("is nothing ");
        }

        private void OnDestroy()
        {
            luaupdate = null;
            syncfix = null;

            //luaenv.Dispose();
        }
    }


}