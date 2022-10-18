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
            // ClassDiagram.Instance.AddAttribute("Class", attribute);
        }
        public void SetClassName(string className, ulong id)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                SetClassNameClientRpc(className, id);
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                SetClassNameServerRpc(className, id);
            }
        }

        [ClientRpc]
        public void SetClassNameClientRpc(string className, ulong id)
        {
            if (NetworkManager.Singleton.IsServer)
                return;
            Debug.Log("Client: Setting class name. Id: " + id + "name: " + className);
            ClassDiagramModel.Instance.SetClassName(className, id);
            var no = NetworkManager.FindObjectsOfType<NetworkObject>();
            foreach (var obj in no)
            {
                var networkObjectId = obj.GetComponent<NetworkObject>().NetworkObjectId;
                if (networkObjectId == id)
                {
                    var background = obj.transform.Find("Background");
                    var header = background.Find("Header");
                    header.GetComponent<TextMeshProUGUI>().text = className;
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetClassNameServerRpc(string className, ulong id)
        {
            if (NetworkManager.Singleton.IsClient)
                return;
            ClassDiagramModel.Instance.SetClassName(className, id);
        }

        // Client Rpc is executed only at client.
        [ClientRpc]
        public void AddClassToModelClientRpc(ulong id)
        {
            if (NetworkManager.Singleton.IsServer)
                return;
            Debug.Log("Client: Add class with id: " + id);
            ClassDiagramModel.Instance.AddElement(id);
        }

        [ServerRpc]
        public void AddClassToModelServerRpc(ulong id)
        {
            if (NetworkManager.Singleton.IsClient)
                return;
            Debug.Log("Server: Add class with id: " + id);
            ClassDiagramModel.Instance.AddElement(id);
        }

        // Spawning is only at server side, not at client.
        public ulong SpawnGameObject(GameObject go)
        {
            Debug.Assert(!NetworkManager.Singleton.IsServer);
            var no = go.GetComponent<NetworkObject>();
            no.Spawn();
            return no.NetworkObjectId;
        }
        public void SpawnClass(GameObject go)
        {
            Debug.Assert(!NetworkManager.Singleton.IsServer);
            var id = SpawnGameObject(go);
            AddClassToModelClientRpc((ulong)id);
        }
    }
}
