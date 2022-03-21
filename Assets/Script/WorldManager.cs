
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace Lots
{
    public class WorldManager : MonoBehaviour
    {
        //private LotsNetworkManager networkManager;

        void Awake()
        {
            // TODO meraz get the manager a bit better :)
            //networkManager = NetworkManager.Singleton.GetComponent<LotsNetworkManager>();
            //Assert.IsNotNull(networkManager,"NetworkManager cannot be null");
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        void StartButtons()
        {
            if (GUILayout.Button("Host"))   NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        static void SubmitNewPosition()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
            {
                if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
                {
                    foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                        NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerClientMovement>().ManualMove();
                }
                else
                {
                    var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                    var player = playerObject.GetComponent<PlayerClientMovement>();
                    player.ManualMove();
                }
            }
        }
    }
}