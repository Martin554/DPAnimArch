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
        private NetworkList<char> m_className;
        // private NetworkVariable<FixedString32Bytes> m_classesCount = new NetworkVariable<FixedString32Bytes>();

        private void Awake()
        {
            m_className = new NetworkList<char>();
        }
        public override void OnNetworkSpawn()
        {
            m_className = new NetworkList<char>();
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

        private void OnClassNameChanged(NetworkListEvent<char> list)
        {
            Debug.Log($"Previous:{list.Value.ToString()} | Current: {list.Value.ToString()}");
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

        [ServerRpc(RequireOwnership = false)]
        public void SetClassNameServerRpc(string name)
        {
            foreach (var ch in name)
                m_className.Add(ch);
        }

    }
}
