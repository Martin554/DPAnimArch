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
    private List<ClassView> ClassViews;
    // private DiagramModel<MetadataClass> classDiagram;

    public List<Class> diagramClasses => DiagramClasses;
    public List<ClassView> classViews => ClassViews;

    //Awake is called before the first frame and before Start()
    private void Awake()
    {
        //Asign memory for variables before the first frame
        GameObjectRelations = new Dictionary<string, GameObject>();
        DiagramClasses = new List<Class>();
        DiagramRelations = new List<Relation>();
        ClassViews = new List<ClassView>();
        ResetDiagram();
        ClassDiagramModel.Instance.ClearDiagram();
    }
    private void Start()
    {
    }
    protected void ResetClasses()
    {
        if (ClassViews != null)
        {
            foreach (ClassView currentClass in ClassViews)
            {
                Destroy(currentClass.GameObject);
            }
            ClassViews.Clear();
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

        ClassDiagramModel.Instance.ClearDiagram();

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
            ClassDiagramModel.Instance.AddClass(currentClass.Name);
            if (NetworkManager.Singleton.IsServer)
            {
                var classGameObject = graph.AddNode();
                Networking.Spawner.Instance.SpawnClass(classGameObject);
                var networkObject = classGameObject.GetComponent<NetworkObject>();
                var classNetworkId = networkObject.NetworkObjectId;

                var classView = new ClassView(classGameObject, classNetworkId);
                classView.Top = currentClass.Top;
                classView.Bottom = currentClass.Bottom;
                classView.Right = currentClass.Right;
                classView.Left = currentClass.Left;
                classView.SetClassName(currentClass.Name);
                classView.SetTMProAttributes(currentClass.Attributes);

                classView.SetTMProMethods(currentClass.Methods);
                ClassDiagram.Instance.ClassViews.Add(classView);

                currentClass.Id = classNetworkId;

                var stringMethods = classView.methodListToString(currentClass.Methods);

                Networking.Spawner.Instance.SetClassName(currentClass.Name, classNetworkId);
                Networking.Spawner.Instance.SetClassMethods(stringMethods, classNetworkId);
            }
            else
            {
                Networking.Spawner.Instance.SpawnClassServerRpc();
                Networking.SharedClassDiagram.Instance.IncrementClassCountServerRpc();
            }
        }


        foreach (var currentRelation in DiagramRelations)
        {
            ClassDiagramModel.Instance.AddRelation(currentRelation);
        }

        GenerateDiagramGameObjects();

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
        foreach (ClassView currentClass in ClassViews)
        {
            currentClass.GameObject.GetComponent<RectTransform>().position = new Vector3(currentClass.Left*1.25f, currentClass.Top*1.25f);
        }
    }
    //Create GameObjects from the parsed data sotred in list of Classes and Relations
    protected void GenerateDiagramGameObjects()
    {
        GenerateRelationGameObjects();
    }


    // Generates GameObjects of relations from member DiagramClasses.
    protected void GenerateRelationGameObjects()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            foreach (Relation rel in DiagramRelations)
            {
                GameObject prefab = rel.PrefabType;
                if (prefab == null)
                {
                    prefab = associationNonePrefab;
                    Debug.Log("Unknown prefab");
                }
                var fromClassId = diagramClasses.Find(item => item.Name == rel.FromClass).Id;
                var toClassId = diagramClasses.Find(item => item.Name == rel.ToClass).Id;

                var fromClass = ClassViews.Find(classView => classView.Id == fromClassId).GameObject;
                var toClass = ClassViews.Find(classView => classView.Id == toClassId).GameObject;

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
    public AttributeModel FindAttributeByName(String searchedClass, String attribute)
    {
        Class c = FindClassByName(searchedClass);
        if (c == null)
            return null;

        if (c.Attributes == null)
            return null;
        
        foreach (AttributeModel atr in c.Attributes)
        {
            if (atr.Name.Equals(attribute))
                return atr;
        }
        Debug.Log("Method " + attribute + "not found");
        return null;
    }
    public bool AddAttribute(String targetClass, AttributeModel atr)
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
                    c.Attributes = new List<AttributeModel>();
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
    public GameObject FindNode(string name)
    {
        var classId = diagramClasses.Find(item => item.Name == name);
        return ClassViews.Find(item => item.Id == classId.Id).GameObject;
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
