using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.Process
{
    [CreateAssetMenu(fileName = "Process", menuName = "ScriptableObjects/Process", order = 1)]
    public class ProcessScriptObject : ScriptableObject
    {
        public List<string> ProcessName;
    }
}