using System;
using Unity.Netcode;
using Unity.Netcode.Transports;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Visualization.Networking
{
    public class PlayerManager : NetworkSingleton<PlayerManager>
    {
        static string playerName = "Enter name";
        string ip = "127.0.0.1";
        ushort port = 55555;

        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += (id) => { };
        }
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                playerName = GUILayout.TextField(playerName, 25);
                ip = GUILayout.TextField(ip, 25);
                var portString = GUILayout.TextField(port.ToString(), 7);
                port = (ushort)UInt16.Parse(portString);
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            GUILayout.EndArea();
        }

        void StartButtons()
        {
            if (GUILayout.Button("Host"))
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, port, "0.0.0.0");
                NetworkManager.Singleton.StartHost();
                SceneManager.LoadScene("AnimArch");
            }
            else if (GUILayout.Button("Client"))
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, port);
                NetworkManager.Singleton.StartClient();
            }
            else if (GUILayout.Button("Server"))
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, port, "0.0.0.0");
                NetworkManager.Singleton.StartServer();
                SceneManager.LoadScene("AnimArch");
            }
        }

        void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
            GUILayout.Label("Server IP: " + ip);
            GUILayout.Label("Server port: " + port);
            GUILayout.Label("Mode: " + mode);
        }
    }
}
