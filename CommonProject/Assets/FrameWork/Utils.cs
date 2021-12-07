using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.Common
{
    public class Utils
    {
        public static string GetBuildVersion()
        {
            return Resources.Load<GameConfig>("GameConfig").BuildVersion;
        }
        
        public static string GetAssetVersion()
        {
            return Resources.Load<GameConfig>("GameConfig").AssetVersion;
        }
    }
}
