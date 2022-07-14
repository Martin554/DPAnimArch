using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [ServerRpc(RequireOwnership = false)]
        public void SpawnClassServerRpc(ServerRpcParams rpcParams = default)
        {
            Debug.Log("Executing spawn class RPC");
            ClassEditor.Instance.CreateNode();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetClassNameServerRpc(String className)
        {
            ClassEditor.Instance.CreateNode();
        }

        public void SpawnClass(GameObject go)
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                return;
            }
            go.GetComponent<NetworkObject>().Spawn();
        }


    }
}
