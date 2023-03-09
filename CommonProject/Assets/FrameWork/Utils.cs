using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FrameWork
{
    public class Utils
    {
        public static List<string> GetClassName(Type type)
        {
            var typeList = new List<Type>();
            var assembly = type.Assembly;
            var assemblyAllTypes = assembly.GetTypes();
            foreach (var assemblyType in assemblyAllTypes)
            {
                var baseType = assemblyType.BaseType;
                if (baseType != null && baseType == type)
                {
                    typeList.Add(assemblyType);
                }
            }

            return typeList.Select(item => item.Name).ToList();
        }
    }
}
