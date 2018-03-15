using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace sdy.Pool
{

    public class ReusePoolSingleton
    {

        private static ReusePoolSingleton instance = null;
        private ReusePoolSingleton() { }

        public static ReusePoolSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new ReusePoolSingleton();
            }
            return instance;
        }


        private Dictionary<string, List<GameObject>> poolDic;
        private Action<GameObject> pReset;
        private Func<GameObject> pNew;

        /// <summary>
        /// 从对应的对象池中取出对象的方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dNew"></param>
        /// <param name="dReset"></param>
        /// <returns></returns>
        public GameObject New(string key, Func<GameObject> dNew, Action<GameObject> dReset = null)
        {
            if (poolDic.ContainsKey(key))
            {
                if (poolDic[key].Count > 0)
                {
                    GameObject go = poolDic[key][0];
                    poolDic[key].Remove(go);
                    if (dReset != null)
                    {
                        dReset(go);
                    }
                    return go;
                }
                else
                {
                    return dNew();
                }
            }
            else
            {
                poolDic.Add(key, new List<GameObject>());
                return dNew();
            }
        }

        /// <summary>
        /// 销毁的对象存入对象池
        /// </summary>
        /// <param name="key"></param>
        /// <param name="go"></param>
        public void Store(string key, GameObject go)
        {
            if (poolDic.ContainsKey(key))
            {
                poolDic[key].Add(go);
            }
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="key"></param>
        public void DestroyPool(string key)
        {
            if (poolDic.ContainsKey(key))
            {
                poolDic.Remove(key);
            }
        }
    }

}
