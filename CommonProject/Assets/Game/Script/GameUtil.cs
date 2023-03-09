//=====================================================
// Autuor: sdy
// Time: 2023/02/23 21:22:09
// Description: 
//=====================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtil
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
