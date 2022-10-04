using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Collections;
using Unity.Collections.LowLevel;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;

namespace Networking
{
    class SharedClassDiagram : NetworkSingleton<SharedClassDiagram>
    {
        private NetworkVariable<int> m_classesCount = new NetworkVariable<int>();

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                m_classesCount.Value = 0;
            }

            m_classesCount.OnValueChanged += OnClassesCountChanged;
        }
        private void OnClassesCountChanged(int previous, int current)
        {
            Debug.Log("OnChange was triggered.");
            //if (IsClient && !IsHost)
            //{
            //}
            if (!IsServer)
                Debug.Log("At client.");
            else
                Debug.Log("At server.");
            Debug.Log($"Detected NetworkVariable Change: Previous: {previous} | Current: {current}");
        }

        public void InceremntClassCount()
        {
            m_classesCount.Value++;
        }

        [ServerRpc(RequireOwnership = false)]
        public void IncrementClassCountServerRpc()
        {
            m_classesCount.Value++;
        }
    }
}
