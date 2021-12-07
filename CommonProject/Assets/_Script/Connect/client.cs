using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/*
public class client : MonoBehaviour
{
    //要连接的服务器地址  
    string IP = "127.0.0.1";
    //要连接的端口  
    int Port = 10000;


    void OnGUI()
    {
        //端类型的状态  
        switch (Network.peerType)
        {
            //禁止客户端连接运行, 服务器未初始化  
            case NetworkPeerType.Disconnected:
                StartConnect();
                break;
            //运行于服务器端  
            case NetworkPeerType.Server:
                break;
            //运行于客户端  
            case NetworkPeerType.Client:
                break;
            //正在尝试连接到服务器  
            case NetworkPeerType.Connecting:
                break;
        }
    }


    void StartConnect()
    {
        if (GUILayout.Button("连接服务器"))
        {
            NetworkConnectionError error = Network.Connect(IP, Port);
            Debug.Log("连接状态" + error);
        }
    }

    // Use this for initialization  
    void Start()
    {

    }

    // Update is called once per frame  
    void Update()
    {

    }
}
*/