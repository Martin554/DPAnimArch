//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Unity.Netcode;

//public class SpawnerControl : NetworkSingleton<SpawnerControl>
//{
//    [SerializeField]
//    private GameObject objectPrefab;

//    [SerializeField]
//    private int maxObjectInstances = 3;

//    private void Awake()
//    {
//        // initial pool
//    }

//    public void SpawnObject()
//    {
//        if (!NetworkManager.Singleton.IsServer)
//        {
//            return;
//        }

//        for (int i = 0; i < maxObjectInstances; i++)
//        {
//            if (objectPrefab == null)
//            {
//                Debug.Log("Object is null");
//                // objectPrefab = Resources.Load<GameObject>("Prefabs/SharedObject");
//                objectPrefab = Resources.Load<GameObject>("Prefabs/Class");
//            }
//            GameObject go = Instantiate(objectPrefab, new Vector3(0, 0, 250), Quaternion.identity);
//            go.GetComponent<NetworkObject>().Spawn();
//            // pool instantiation
//        }

//    }
//}
