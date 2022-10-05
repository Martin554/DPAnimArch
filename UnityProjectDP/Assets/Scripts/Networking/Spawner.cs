using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Unity.Netcode;

namespace Networking
{
    [RequireComponent(typeof(NetworkObject))]
    class Spawner : NetworkSingleton<Spawner>
    {
        [SerializeField]
        private GameObject classPrefab;
       
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            // initial pool
        }

        // Server RPC - if client creates instance, server RPC is called and instance is created at server side.
        [ServerRpc(RequireOwnership = false)]
        public void SpawnClassServerRpc(ServerRpcParams rpcParams = default)
        {
            Debug.Log("Executing spawn class RPC");
            ClassEditor.Instance.CreateNode();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnGraphServerRpc(ServerRpcParams rpcParams = default)
        {
            Debug.Log("Executing spawn graph RPC");
            ClassEditor.Instance.InitializeCreation();
        }

        [ClientRpc]
        public void SetAttributeClientRpc(string attributeText)
        {
            if (NetworkManager.Singleton.IsHost)
                return;
            Debug.Log("Add attribute name: " + attributeText);
            Attribute attribute = new Attribute();
            attribute.Name = attributeText;
            ClassDiagram.Instance.AddAttribute("Class", attribute);
        }

        [ClientRpc]
        public void AddClassToModelClientRpc(string className)
        {
            if (NetworkManager.Singleton.IsHost)
                return;
            Debug.Log("Add class with name: " + className);
        }

        // Spawn Class GameObject over the network. After spawning class, it will be visible for clients.
        public void SpawnGameObject(GameObject go)
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            go.GetComponent<NetworkObject>().Spawn();
        }
    }
}
