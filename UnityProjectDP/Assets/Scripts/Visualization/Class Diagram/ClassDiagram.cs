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
    private Dictionary<string, GameObject> GameObjectClasses; //Dictionary of all ojects created from relations list

    public List<Class> diagramClasses => DiagramClasses;
    public Dictionary<string, GameObject> gameObjectClasses => GameObjectClasses;

    //Awake is called before the first frame and before Start()
    private void Awake()
    {
        //Asign memory for variables before the first frame
        GameObjectClasses = new Dictionary<string, GameObject>();
        GameObjectRelations = new Dictionary<string, GameObject>();
        DiagramClasses = new List<Class>();
        DiagramRelations = new List<Relation>();
        ResetDiagram();
    }
    private void Start()
    {
    }
    protected void ResetClasses()
    {
        if (GameObjectClasses != null && GameObjectClasses.Count > 0)
        {
            foreach (KeyValuePair<string, GameObject> kv in GameObjectClasses)
            {
                Destroy(kv.Value);
            }

            GameObjectClasses.Clear();
        }
        DiagramClasses.Clear();
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
            ParseClassData();
            ParseRelationData();
            k++;
            AnimationData.Instance.diagramId++;
        }
        GenerateDiagramGameObjects();
        ManualLayout();
    }
    public Graph CreateGraph()
    {
        ResetDiagram();
        var go = GameObject.Instantiate(graphPrefab);
        if (NetworkManager.Singleton.IsServer)
        {
            go.GetComponent<NetworkObject>().Spawn();
        }
        graph = go.GetComponent<Graph>();
        return graph;
    }
    // Parse classes data from XMI to DiagramClasses member.
    protected void ParseClassData()
    {
        List<Class> XMIClassList = XMIParser.ParseClasses();
        if (XMIClassList == null)
            XMIClassList = new List<Class>();

        foreach (Class CurrentClass in XMIClassList)
        {
            CurrentClass.Name = CurrentClass.Name.Replace(" ", "_");

            CDClass TempCDClass = null;
            int i = 0;
            string currentName = CurrentClass.Name;
            string baseName = CurrentClass.Name;
            while (TempCDClass == null)
            {
                currentName = baseName + (i == 0 ? "" : i.ToString());
                TempCDClass = OALProgram.Instance.ExecutionSpace.SpawnClass(currentName);
                i++;
                if (i > 1000)
                {
                    break;
                }
            }
            CurrentClass.Name = currentName;
            if (TempCDClass == null)
                continue;

            if (CurrentClass.Attributes != null)
            {
                foreach (Attribute CurrentAttribute in CurrentClass.Attributes)
                {
                    CurrentAttribute.Name = CurrentAttribute.Name.Replace(" ", "_");
                    String AttributeType = EXETypes.ConvertEATypeName(CurrentAttribute.Type);
                    if (AttributeType == null)
                        continue;
                    TempCDClass.AddAttribute(new CDAttribute(CurrentAttribute.Name, EXETypes.ConvertEATypeName(AttributeType)));
                    if (CurrentClass.attributes == null)
                        CurrentClass.attributes = new List<Attribute>();
                }
            }

            if (CurrentClass.Methods != null)
            {
                foreach (Method CurrentMethod in CurrentClass.Methods)
                {
                    CurrentMethod.Name = CurrentMethod.Name.Replace(" ", "_");
                    CDMethod Method = new CDMethod(TempCDClass, CurrentMethod.Name, EXETypes.ConvertEATypeName(CurrentMethod.ReturnValue));
                    TempCDClass.AddMethod(Method);

                    foreach (string arg in CurrentMethod.arguments)
                    {
                        string[] tokens = arg.Split(' ');
                        string type = tokens[0];
                        string name = tokens[1];

                        Method.Parameters.Add(new CDParameter() { Name = name, Type = EXETypes.ConvertEATypeName(type) });
                    }
                }
            }
            CurrentClass.Top *= -1;
            DiagramClasses.Add(CurrentClass);
        }
    }
    // Parse relations data from XMI to DiagramRelations member.
    protected void ParseRelationData()
    {
        List<Relation> XMIRelationList = XMIParser.ParseRelations();
        if (XMIRelationList == null)
        {
            XMIRelationList = new List<Relation>();
        }
        
        foreach (Relation Relation in XMIRelationList)
        {
            Relation.FromClass = Relation.SourceModelName.Replace(" ", "_");
            Relation.ToClass = Relation.TargetModelName.Replace(" ", "_");
            // Here you assign prefabs for each type of relation
            switch (Relation.PropertiesEa_type)
            {
                case "Association": switch (Relation.ProperitesDirection)
                    {
                        case "Source -> Destination": Relation.PrefabType = associationSDPrefab; break;
                        case "Destination -> Source": Relation.PrefabType = associationDSPrefab; break;
                        case "Bi-Directional": Relation.PrefabType = associationFullPrefab; break;
                        default: Relation.PrefabType = associationNonePrefab; break;
                    }
                    break;
                case "Generalization": Relation.PrefabType = generalizationPrefab; break;
                case "Dependency": Relation.PrefabType = dependsPrefab; break;
                case "Realisation": Relation.PrefabType = realisationPrefab; break;
                default: Relation.PrefabType = associationNonePrefab; break;
            }

            CDRelationship TempCDRelationship = OALProgram.Instance.RelationshipSpace.SpawnRelationship(Relation.FromClass, Relation.ToClass);
            Relation.OALName = TempCDRelationship.RelationshipName;

            if ("Generalization".Equals(Relation.PropertiesEa_type) || "Realisation".Equals(Relation.PropertiesEa_type))
            {
                CDClass FromClass = OALProgram.Instance.ExecutionSpace.getClassByName(Relation.FromClass);
                CDClass ToClass = OALProgram.Instance.ExecutionSpace.getClassByName(Relation.ToClass);

                if (FromClass != null && ToClass != null)
                    FromClass.SuperClass = ToClass;
            }

            DiagramRelations.Add(Relation);
        }
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
        foreach (Class c in DiagramClasses)
        {
            GameObjectClasses[c.Name].GetComponent<RectTransform>().position = new Vector3(c.Left*1.25f, c.Top*1.25f);
        }
    }
    //Create GameObjects from the parsed data sotred in list of Classes and Relations
    protected void GenerateDiagramGameObjects()
    {
        Debug.Log("DIAGRAM CLASSES COUNT" + DiagramClasses.Count);
        Debug.Log("RELATION COUNT" + DiagramRelations.Count);
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
                var node = graph.AddNode();
                node.name = DiagramClasses[i].Name;
                var background = node.transform.Find("Background");
                var header = background.Find("Header");
                var attributes = background.Find("Attributes");
                var methods = background.Find("Methods");

                // Print name of class
                header.GetComponent<TextMeshProUGUI>().text = DiagramClasses[i].Name;

                //Attributes
                if (DiagramClasses[i].Attributes != null)
                {
                    foreach (Attribute attr in DiagramClasses[i].Attributes)
                    {
                        attributes.GetComponent<TextMeshProUGUI>().text += attr.Name + ": " + attr.Type + "\n";
                    }
                }

                //Methods
                if (DiagramClasses[i].Methods != null)
                {
                    foreach (Method method in DiagramClasses[i].Methods)
                    {
                        string arguments = "(";
                        if (method.arguments != null)
                        {
                            for (int argumentIndex = 0; argumentIndex < method.arguments.Count; argumentIndex++)
                            {
                                if (argumentIndex < method.arguments.Count - 1)
                                    arguments += (method.arguments[argumentIndex] + ", ");
                                else
                                    arguments += (method.arguments[argumentIndex]);
                            }
                        }
                        arguments += ")";
                        methods.GetComponent<TextMeshProUGUI>().text += method.Name + arguments + " :" + method.ReturnValue + "\n";
                    }
                }
                GameObjectClasses.Add(node.name, node);
                Networking.Spawner.Instance.SpawnClass(node);
                // Networking.Spawner.Instance.SetClassNameClientRpc("Abcd");
                //Debug.Log(node.name);
            }
            else
            {
                Networking.Spawner.Instance.SpawnClassServerRpc();
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
            if (GameObjectClasses.TryGetValue(rel.FromClass, out g) && GameObjectClasses.TryGetValue(rel.ToClass, out g))
            {
                GameObject edge = graph.AddEdge(GameObjectClasses[rel.FromClass], GameObjectClasses[rel.ToClass], prefab);
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
        Class result=null;
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
            if (FindMethodByName(targetClass, methodToAdd.Name)==null)
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
    public bool AddAtr(String targetClass, Attribute atr)
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
        return true;
    }
    public GameObject FindNode(String name)
    {
        return GameObjectClasses[name];
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
