﻿using UnityEngine;

namespace FrameWork.Common
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    { 
        public string BuildVersion;
        public string AssetVersion; 
    }
}