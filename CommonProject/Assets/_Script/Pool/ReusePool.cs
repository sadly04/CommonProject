using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace sdy.Pool
{

    public class ReusePool <T> where T : class 
    {

        private Action<T> pReset;                          //重置对象的委托
        private Func<T> pNew;                              //创建新对象的委托
        private Stack<T> stack;                            //存放对象的池子,堆


        public ReusePool(Func<T> dnew, Action<T> dReset = null)
        {
            pReset = dReset;
            pNew = dnew;
            stack = new Stack<T>();
        }

        /// <summary>
        /// 从池子中获取对象的方法，若池子的数量为0，则调用创建新对象的委托创建一个
        /// 对象返回，否则从池子中拿出一个对象返回
        /// </summary>
        /// <returns></returns>
        public T New()
        {
            if (stack.Count == 0)
            {
                T t = pNew();
                return t;
            }
            else
            {
                T t = stack.Pop();
                if (pReset != null)
                {
                    pReset(t);
                }
                return t;
            }
        }

        /// <summary>
        /// 用于将销毁的对象存入池子
        /// </summary>
        /// <param name="t"></param>
        public void Store(T t)
        {
            stack.Push(t);
        }

        /// <summary>
        /// 清空池子
        /// </summary>
        public void Clear()
        {
            stack.Clear();
        }
    }

}
