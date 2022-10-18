using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using System.Xml;
using OALProgramControl;
using Unity.Netcode;

public class ClassDiagram : Singleton<ClassDiagram>
{
    public GameObject graphPrefab;
    public GameObject classPrefab;
    public GameObject associationNonePrefab;
    public GameObject associationFullPrefab;
    public GameObject associationSDPrefab;
    public GameObject associationDSPrefab;
    public GameObject dependsPrefab;
    public GameObject generalizationPrefab;
    public GameObject implementsPrefab;
    public GameObject realisationPrefab;
    public Graph graph;
    private List<Class> DiagramClasses; //List of all classes from XMI
    private List<Relation> DiagramRelations; //List of all relations from XMI
    private Dictionary<string, GameObject> GameObjectRelations; // Dictionary of all objects created from list classes

    // private DiagramModel<MetadataClass> classDiagram;

    public List<Class> diagramClasses => DiagramClasses;

    //Awake is called before the first frame and before Start()
    private void Awake()
    {
        //Asign memory for variables before the first frame
        GameObjectRelations = new Dictionary<string, GameObject>();
        DiagramClasses = new List<Class>();
        DiagramRelations = new List<Relation>();
        ResetDiagram();
        ClassDiagramModel.Instance.ResetDiagram();
    }
    private void Start()
    {
    }
    protected void ResetClasses()
    {
        if (diagramClasses != null)
        {
            foreach (Class currentClass in diagramClasses)
            {
                Destroy(currentClass.GameObject);
            }
            diagramClasses.Clear();
        }
    }
    protected void ResetRelations()
    {
        if (GameObjectRelations != null && GameObjectRelations.Count > 0)
        {
            foreach (KeyValuePair<string, GameObject> kv in GameObjectRelations)
            {
                Destroy(kv.Value);
            }

            GameObjectRelations.Clear();
        }

        DiagramRelations.Clear();
    }
    protected void ResetGraph()
    {
        if (graph != null)
        {
            Destroy(graph.gameObject);
            graph = null;
        }
    }
    public void ResetDiagram()
    {
        ResetClasses();
        ResetRelations();
        ResetGraph();

        ClassDiagramModel.Instance.ResetDiagram();

        OALProgram.Instance.ExecutionSpace.ClassPool.Clear();
        OALProgram.Instance.ExecutionSpace= new CDClassPool();
        OALProgram.Instance.RelationshipSpace = new CDRelationshipPool();
        AnimationData.Instance.ClearData();
    }
    public void LoadDiagram()
    {
        CreateGraph();
        int k = 0;
        // A trick used to skip empty diagrams in XMI file from EA
        while (DiagramClasses.Count < 1 && k < 10)
        {
            // View - generate GameObjects + CD data
            DiagramClasses = ClassDiagramGenerator.Instance.GenerateClassesData(ref graph);
            DiagramRelations = ClassDiagramGenerator.Instance.GenerateRelationsData();
            k++;
            AnimationData.Instance.diagramId++;
        }

        foreach(var currentClass in DiagramClasses)
        {
            ClassDiagramModel.Instance.AddElement(currentClass.Name);
            // Networking.Spawner.Instance.AddClassToModelClientRpc(currentClass.Name);
        }

        GenerateDiagramGameObjects();

        foreach (var currentRelation in DiagramRelations)
        {
            ClassDiagramModel.Instance.AddRelation(currentRelation);
        }

        ManualLayout();
    }
    public Graph CreateGraph()
    {
        ResetDiagram();
        var graphGameObject = GameObject.Instantiate(graphPrefab);
        graph = graphGameObject.GetComponent<Graph>();
        if (NetworkManager.Singleton.IsServer)
        {
            Networking.Spawner.Instance.SpawnGameObject(graphGameObject);
        }
        else
        {
            Networking.Spawner.Instance.SpawnClassServerRpc();
        }
        return graph;
    }

    //Auto arrange objects in space
    public void AutoLayout()
    {
        //TODO better automatic Layout
        graph.Layout();
    }
    //Set layout as close as possible to EA layout
    public void ManualLayout()
    {
        foreach (Class currentClass in DiagramClasses)
        {
            currentClass.GameObject.GetComponent<RectTransform>().position = new Vector3(currentClass.Left*1.25f, currentClass.Top*1.25f);
        }
    }
    //Create GameObjects from the parsed data sotred in list of Classes and Relations
    protected void GenerateDiagramGameObjects()
    {
        GenerateClassesGameObjects();
        GenerateRelationGameObjects();
    }


    // Generates GameObjects of classes from member DiagramClasses.
    protected void GenerateClassesGameObjects()
    {
        for (int i = 0; i < DiagramClasses.Count; i++)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                Networking.Spawner.Instance.SpawnClass(DiagramClasses[i].GameObject);
                Networking.SharedClassDiagram.Instance.InceremntClassCount();

                var networkObject = DiagramClasses[i].GameObject.GetComponent<NetworkObject>();

                Networking.Spawner.Instance.SetClassName(DiagramClasses[i].Name, networkObject.NetworkObjectId);
            }
            else
            {
                Networking.Spawner.Instance.SpawnClassServerRpc();
                Networking.SharedClassDiagram.Instance.IncrementClassCountServerRpc();
            }
        }
    }
    // Generates GameObjects of relations from member DiagramClasses.
    protected void GenerateRelationGameObjects()
    {
        foreach (Relation rel in DiagramRelations)
        {
            GameObject prefab = rel.PrefabType;
            if (prefab == null)
            {
                prefab = associationNonePrefab;
                Debug.Log("Unknown prefab");
            }
            GameObject g;
            var fromClass = diagramClasses.Find(item => item.Name == rel.FromClass).GameObject;
            var toClass = diagramClasses.Find(item => item.Name == rel.ToClass).GameObject;
            if (fromClass && toClass)
            {
                GameObject edge = graph.AddEdge(fromClass, toClass, prefab);
                GameObjectRelations.Add(rel.OALName, edge);
                if (edge.gameObject.transform.childCount > 0)
                    StartCoroutine(QuickFix(edge.transform.GetChild(0).gameObject));
            }
            else
                Debug.Log("Cant find specified Edge in Dictionary");
        }
    }
    public Class FindClassByName(String searchedClass)
    {
        Class result = null;
        foreach(Class c in DiagramClasses)
        {
            if (c.Name.Equals(searchedClass))
            {
                result = c;
                Debug.Log("result:"+c.Name);
                return result;
            }
        }
        
        Debug.Log("Class " + searchedClass+ " not found");

        return result;
    }
    public Method FindMethodByName(String searchedClass,String searchedMethod)
    {
        Class c = FindClassByName(searchedClass);
        if (c == null)
            return null;
        
        if (c.Methods == null)
            return null;
        
        foreach (Method m in c.Methods)
        {
            if (m.Name.Equals(searchedMethod))
            {
                return m;
            }
        }
        Debug.Log("Method " + searchedMethod + "not found");
        return null;
    }
    public bool AddMethod(String targetClass, Method methodToAdd)
    {
        Class c = FindClassByName(targetClass);
        if (c == null)
            return false;
        else
        {
            if (FindMethodByName(targetClass, methodToAdd.Name) == null)
            {
                if (c.Methods == null)
                {
                    c.Methods = new List<Method>();
                }
                c.Methods.Add(methodToAdd);
                if (OALProgram.Instance.ExecutionSpace.ClassExists(targetClass))
                {
                    CDClass CDClass = OALProgram.Instance.ExecutionSpace.getClassByName(targetClass);
                    CDClass.AddMethod(new CDMethod(CDClass, methodToAdd.Name, methodToAdd.ReturnValue));
                }
                
            }
            else
                return false;
        }
        return true;
    }
    public Attribute FindAttributeByName(String searchedClass, String attribute)
    {
        Class c = FindClassByName(searchedClass);
        if (c == null)
            return null;

        if (c.Attributes == null)
            return null;
        
        foreach (Attribute atr in c.Attributes)
        {
            if (atr.Name.Equals(attribute))
                return atr;
        }
        Debug.Log("Method " + attribute + "not found");
        return null;
    }
    public bool AddAttribute(String targetClass, Attribute atr)
    {
        Class c = FindClassByName(targetClass);
        if (c == null)
            return false;
        else
        {
            if (FindAttributeByName(targetClass, atr.Name) == null)
            {
                if (c.Attributes == null)
                {
                    c.Attributes = new List<Attribute>();
                }
                c.Attributes.Add(atr);
            }
            else return false;

        }
        if (NetworkManager.Singleton.IsServer)
        {
            Networking.Spawner.Instance.SetAttributeClientRpc(atr.Name);
        }
        return true;
    }
    public GameObject FindNode(String name)
    {
        return diagramClasses.Find(item => item.Name == name).GameObject;
    }
    public GameObject FindEdge(string classA, string classB)
    {
        CDRelationship Rel = OALProgram.Instance.RelationshipSpace.GetRelationshipByClasses(classA, classB);
        if (Rel != null)
            return FindEdge(Rel.RelationshipName);

        return null;
    }
    public GameObject FindEdge(string RelationshipName)
    {
        if (GameObjectRelations.ContainsKey(RelationshipName))
            return GameObjectRelations[RelationshipName];

        return null;
    }
    public String FindOwnerOfRelation(String RelationName)
    {
        foreach (Relation Relation in DiagramRelations)
        {
            if (String.Equals(Relation.OALName, RelationName))
                return Relation.FromClass;
        }
        return "";
    }
    //Fix used to minimalize relation displaying bug
    private IEnumerator QuickFix(GameObject g)
    {
        yield return new WaitForSeconds(0.05f);
        g.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        g.SetActive(true); 
    }
    public List<Class> GetClassList()
    {
        return DiagramClasses;
    }
    public List<Relation> GetRelationList()
    {
        return DiagramRelations;
    }
    public void CreateRelationEdge(GameObject node1, GameObject node2)
    {
        GameObject edge = graph.AddEdge(node1, node2, associationFullPrefab);
    }
}
