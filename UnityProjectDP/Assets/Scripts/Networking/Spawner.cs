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
        public void SetClassNameServerRpc(String className, int id)
        {
            Debug.Log("setting name1");
            var cls = ClassDiagram.Instance.diagramClasses;
            if (ClassDiagram.Instance.diagramClasses[id] != null)
            {
                ClassDiagram.Instance.diagramClasses[id].Name = className;
                Debug.Log("setting name2");
            }
        }

        [ClientRpc]
        public void SetClassNameClientRpc(String className)
        {
            Debug.Log("setting name1");
            //var classes = ClassDiagram.Instance.diagramClasses;
            //Class cls = new Class();
            //cls.Name = className;
            //classes.Add(cls);
            //if (classes.Count() > 0 && classes[0] != null)
            //{
            //    ClassDiagram.Instance.diagramClasses[0].Name = className;
            //}
        }

        public void SpawnClass(GameObject go)
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                return;
            }
            // else is client
            go.GetComponent<NetworkObject>().Spawn();
        }
    }
}
