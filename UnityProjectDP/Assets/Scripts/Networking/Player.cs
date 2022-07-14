using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Networking
{
    class Player : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public Color clr;
        public string Name;

        [SerializeField]
        private NetworkVariable<ulong> playerNetworkName = new NetworkVariable<ulong>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {

            }
            if (IsServer)
            {
                playerNetworkName.Value = OwnerClientId;
            }
        }

        //[ServerRpc]
        //public void SpawnObjectsServerRpc(ServerRpcParams rpcParams = default)
        //{
        //    Debug.Log("Executing spawn RPC");
        //    Spawner.Instance.SpawnClass();
        //}
    }
}
