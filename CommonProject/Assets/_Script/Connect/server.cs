using UnityEngine;
using System.Collections;

public class server : MonoBehaviour
{

    int Port = 10000;

    //OnGUI方法，所有GUI的绘制都需要在这个方法中实现   
    void OnGUI()
    {
        //Network.peerType是端类型的状态:   
        //即disconnected, connecting, server 或 client四种   
        switch (Network.peerType)
        {
            //禁止客户端连接运行, 服务器未初始化   
            case NetworkPeerType.Disconnected:
                StartServer();
                break;
            //运行于服务器端   
            case NetworkPeerType.Server:
                OnServer();
                break;
            //运行于客户端   
            case NetworkPeerType.Client:
                break;
            //正在尝试连接到服务器   
            case NetworkPeerType.Connecting:
                break;
        }
    }

    void StartServer()
    {
        //当用户点击按钮的时候为true   
        if (GUILayout.Button("创建服务器"))
        {
            //初始化本机服务器端口，第一个参数就是本机接收多少连接   
            NetworkConnectionError error = Network.InitializeServer(12, Port, false);
            Debug.Log("错误日志" + error);
        }
    }

    void OnServer()
    {
        GUILayout.Label("服务端已经运行,等待客户端连接");
        //Network.connections是所有连接的玩家, 数组[]   
        //取客户端连接数.    
        int length = Network.connections.Length;
        //按数组下标输出每个客户端的IP,Port   
        for (int i = 0; i < length; i++)
        {
            GUILayout.Label("客户端" + i);
            GUILayout.Label("客户端ip" + Network.connections[i].ipAddress);
            GUILayout.Label("客户端端口" + Network.connections[i].port);
        }
        //当用户点击按钮的时候为true   
        if (GUILayout.Button("断开服务器"))
        {
            Network.Disconnect();
        }
    }


    /* 系统提供的方法，该方法只执行一次 */
    // Use this for initialization   
    void Start()
    {

    }

    // Update is called once per frame   
    void Update()
    {

    }
}
