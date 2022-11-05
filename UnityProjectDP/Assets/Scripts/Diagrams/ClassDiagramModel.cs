using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClassDiagramModel : Singleton<ClassDiagramModel>
{
    private List<ClassModel> classes;
    private List<Relation> relations;

    protected override void OnAwake()
    {
        classes = new List<ClassModel>();
        relations = new List<Relation>();
    }

    public virtual void ClearDiagram()
    {
        classes.Clear();
        relations.Clear();
    }

    public void SetClassName(string name, ulong id)
    {
        var metadataClass = classes.Find(x => x.Id == id);
        if (metadataClass != null)
        {
            metadataClass.Name = name;
        }
    }
    public void SetClassMethods(List<Method> methods, ulong id)
    {
        var metadataClass = classes.Find(x => x.Id == id);
        if (metadataClass != null)
        {
            metadataClass.Methods = methods;
        }
    }
    public void AddClass(ClassModel element)
    {
        classes.Add(element);
    }
    public void AddClass(ulong id)
    {
        classes.Add(new ClassModel(id));
    }
    public void AddClass(string name)
    {
        classes.Add(new ClassModel(name));
    }
    public void AddRelation(Relation relation)
    {
        relations.Add(relation);
    }
}
