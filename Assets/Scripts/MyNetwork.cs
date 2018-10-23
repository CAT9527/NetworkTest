using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNetwork : MonoBehaviour {


    public int connections = 10;
    public int listenPort = 8899;  //端口号，不常用的端口
    public bool useNet = false;    //不使用Nat技术
    public string ip = "127.0.0.1";
    public GameObject cube;

    //public static NetworkConnectionError InitializeServer(int connections, int listenPort, bool useNat);
    /// <summary>
    /// 代码解释
    /// connections是允许的传入连接数（请注意，这通常与玩家数量不同）。
    /// listenPort是我们想要收听的端口号。 
    /// useNat设置NAT穿透功能。 如果您希望此服务器能够使用NAT穿透接受连接，请使用辅助器将其设置为true。
    /// </summary>

    void OnGUI()
    {
        //Network.peerType 对等类型的状态，即它是否已断开连接，连接，服务器或客户端。
        //NetworkPeerType枚举类型，包括未连接（Disconnected），连接（Connecting），服务器（Server）,客户端（Client）
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            if (GUILayout.Button("创建服务器"))
            {
                //进行创建服务器的操作
                NetworkConnectionError error = Network.InitializeServer(connections, listenPort, useNet);
                print(error);


            }

            if (GUILayout.Button("连接服务器"))
            {
                //进行连接服务器的操作
                NetworkConnectionError error =  Network.Connect(ip, listenPort);
                print(error);
            }
        }
        else if (Network.peerType == NetworkPeerType.Server)
        {
            GUILayout.Label("服务器创建完成");
        }else if (Network.peerType == NetworkPeerType.Client)
        {
            GUILayout.Label("客户端已接入");
        }

    }
   

    //注意：这两个方法都是服务器端调用的
     void OnServerInitialized()
    {
        print("Server完成初始化");
        //Network.player;//访问到当前的客户端
        int group = int.Parse(Network.player + " ");//直接访问Network.Player会得到当前客户端的索引，这是唯一的

        //实例化Prefab不能再用GameObject.Instantiate，而是应该使用Network.Instantiate
        //Network.Instantiate
        //public static Object Instantiate(Object prefab, Vector3 position, Quaternion rotation, int group);
        //要实例化的Prefab，位置信息，旋转信息，客户端索引信息
        Network.Instantiate(cube, new Vector3(0, 10, 0), Quaternion.identity,group);
    }

     void OnPlayerConnected(NetworkPlayer player)
    {
        print("一个客户端连接过来了，Index  Number : "+player);
    }

    //这个方法是客户端上调用的
    void OnConnectedToServer()
    {
        print("我成功连接到服务器了");
        int group = int.Parse(Network.player + " ");
        Network.Instantiate(cube, new Vector3(0, 10, 0), Quaternion.identity, group);
    }

    //network view 组件用来在局域网内同步一个游戏物体的组件属性
    //network view 会把创建它出来的客户端作为主人，就是主客户端，其他的客户端的数据都会以主客户端的数据为主

}
