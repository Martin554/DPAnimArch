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

        public void SetClassProperty(string propertyName, string property, ulong id)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                SetClassPropertyClientRpc(propertyName, property, id);
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                SetClassPropertyServerRpc(propertyName, property, id);
            }
        }
        [ClientRpc]
        public void SetClassPropertyClientRpc(string propertyName, string property, ulong id)
        {
            if (NetworkManager.Singleton.IsServer)
                return;
            Debug.Log("Client: Setting class " + propertyName + "id: " + id + ", " + property);

            var no = NetworkManager.FindObjectsOfType<NetworkObject>();
            foreach (var obj in no)
            {
                var networkObjectId = obj.GetComponent<NetworkObject>().NetworkObjectId;
                if (networkObjectId == id)
                {
                    var background = obj.transform.Find("Background");
                    var propertyComponent = background.Find(propertyName);
                    propertyComponent.GetComponent<TextMeshProUGUI>().text = property;
                }
            }

            switch (propertyName)
            {
                case "Name":
                {
                    ClassDiagramModel.Instance.SetClassName(property, id);
                    break;
                }
                case "Attribute":
                {
                    break;
                }
                case "Method":
                {
                    break;
                }
            }
        }

        [ClientRpc]
        public void AddEdgeToModelClientRpc(ulong fromClassId, ulong toClassId, string relationType, string direction)
        {
            if (NetworkManager.Singleton.IsServer)
                return;
            var relationTypePrefab = ClassDiagramGenerator.Instance.GenerateRelationTypePrefab(relationType, direction);

            NetworkObject fromClassNetworkObject = null;
            NetworkObject toClassNetworkObject = null;
            NetworkObject graphNetworkObject = null;

            //var gameObjects = NetworkManager.GetComponents<NetworkObject>();
            var gameObjects = NetworkManager.SpawnManager.SpawnedObjects;
            if (gameObjects.Count > 0)
            {
                gameObjects.TryGetValue(fromClassId, out fromClassNetworkObject);
                gameObjects.TryGetValue(toClassId, out toClassNetworkObject);
                gameObjects.TryGetValue(ClassDiagramModel.Instance.GraphId, out graphNetworkObject);

            }

            if (fromClassNetworkObject && toClassNetworkObject && graphNetworkObject)
            {
                var graph = graphNetworkObject.GetComponent<Graph>();
                graph.AddEdge(fromClassNetworkObject.GetComponent<UNode>(), toClassNetworkObject.GetComponent<UNode>(), relationTypePrefab);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetClassPropertyServerRpc(string propertyName, string property, ulong id)
        {

        }

        // Client Rpc is executed only at client.
        [ClientRpc]
        public void AddClassToModelClientRpc(ulong id)
        {
            if (NetworkManager.Singleton.IsServer)
                return;
            Debug.Log("Client: Add class with id: " + id);
            ClassDiagramModel.Instance.AddClass(id);
        }

        [ServerRpc]
        public void AddClassToModelServerRpc(ulong id)
        {
            if (NetworkManager.Singleton.IsClient)
                return;
            Debug.Log("Server: Add class with id: " + id);
            ClassDiagramModel.Instance.AddClass(id);
        }

        // Spawning is only at server side, not at client.
        public ulong SpawnGameObject(GameObject go)
        {
            Debug.Assert(NetworkManager.Singleton.IsServer);
            var no = go.GetComponent<NetworkObject>();
            no.Spawn();
            return no.NetworkObjectId;
        }

        [ClientRpc]
        public void AddGraphToModelClientRpc(ulong graphId)
        {
            ClassDiagramModel.Instance.GraphId = graphId;
        }
        public void SpawnClass(GameObject go)
        {
            Debug.Assert(NetworkManager.Singleton.IsServer);
            var id = SpawnGameObject(go);
            AddClassToModelClientRpc((ulong)id);
        }
    }
}
