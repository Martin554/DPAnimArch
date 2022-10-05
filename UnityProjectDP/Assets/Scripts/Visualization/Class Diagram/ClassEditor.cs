using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class ClassEditor : Singleton<ClassEditor>
{
    Graph graph;
    public int Id { get; set; } = 0;
    bool active = false;
    GameObject node1;
    GameObject node2;

    public GameObject methodMenu;
    public GameObject attributeMenu;
    public AttributeMenu atrMenu;
    public MethodMenu mtdMenu;
    public void InitializeCreation()
    {
        graph = ClassDiagram.Instance.CreateGraph();
        active = true;
    }
    public void CreateNode()
    {
        if (graph == null)
        {
            InitializeCreation();
        }
        if (NetworkManager.Singleton.IsServer)
        {
            var currentClass = ClassDiagramGenerator.Instance.GenerateClass(ref graph);
            RectTransform rc = currentClass.GameObject.GetComponent<RectTransform>();
            rc.position = new Vector3(100f, 200f, 1);

            Networking.Spawner.Instance.SpawnGameObject(currentClass.GameObject);
            Networking.Spawner.Instance.AddClassToModelClientRpc(currentClass.Name);
            ClassDiagram.Instance.diagramClasses.Add(currentClass);
            Id++;
        }
        else
        {
            Networking.Spawner.Instance.SpawnClassServerRpc();
        }
    }

    public void SelectNode(GameObject selected)
    {
        if (active)
        {
            if (node1 == null)
            {
                node1 = selected;
                Debug.Log("node 1 added");
            }
            else if (node2 == null)
            {
                node2 = selected;
                Debug.Log("node 2 added");
            }
            else
            {
                node2 = node1;
                node1 = selected;
            }
        }
    }
    public void DrawRelation()
    {
        if(node1 != null && node2 != null)
        {
            ClassDiagram.Instance.CreateRelationEdge(node1, node2);
            node1 = null;
            node2 = null;
            graph.UpdateGraph();
        }
    }
}
