//=====================================================
// Autuor: sdy
// Time: 2023/02/23 21:45:08
// Description: 
//=====================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameWork;

public class GameApp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GameUtil.GetBuildVersion());
        /*
        var types = Utils.GetClassName(IProcess);
        for (int i = 0; i < types.Count; i++)
        {
            Debug.Log(types[i]);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
