﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using OALProgramControl;


public class ClassDiagramGenerator : Singleton<ClassDiagramGenerator>
{
    public List<Class> GenerateClassesData(ref Graph graph)
    {
        List<Class> classes = XMIParser.ParseClasses();
        if (classes == null)
            return new List<Class>();

        foreach (Class currentClass in classes)
        {
            currentClass.Name = currentClass.Name.Replace(" ", "_");

            CDClass tempCDClass = null;
            int i = 0;
            string currentName = currentClass.Name;
            string baseName = currentClass.Name;
            while (tempCDClass == null)
            {
                currentName = baseName + (i == 0 ? "" : i.ToString());
                tempCDClass = OALProgram.Instance.ExecutionSpace.SpawnClass(currentName);
                i++;
                if (i > 1000)
                {
                    break;
                }
            }
            currentClass.Name = currentName;
            if (tempCDClass == null)
                continue;


            if (currentClass.Attributes != null)
            {
                GenerateClassAttributes(currentClass, ref tempCDClass);
            }

            if (currentClass.Methods != null)
            {
                GenerateClassMethods(currentClass, ref tempCDClass);
            }
            
            currentClass.GameObject = graph.AddNode();

            Debug.Assert(currentClass.GameObject);

            currentClass.SetTMProAttributes();
            currentClass.SetTMProMethods();
            
            currentClass.Top *= -1;
        }
        return classes;
    }
    public List<Relation> GenerateRelationsData()
    {
        List<Relation> relations = XMIParser.ParseRelations();
        if (relations == null)
            return new List<Relation>();

        foreach (Relation relation in relations)
        {
            relation.FromClass = relation.SourceModelName.Replace(" ", "_");
            relation.ToClass = relation.TargetModelName.Replace(" ", "_");
            relation.PrefabType = GenerateRelationTypePrefab(relation.PropertiesEa_type, relation.ProperitesDirection);

            CDRelationship tempCDRelationship = OALProgram.Instance.RelationshipSpace.SpawnRelationship(relation.FromClass, relation.ToClass);
            relation.OALName = tempCDRelationship.RelationshipName;

            if ("Generalization".Equals(relation.PropertiesEa_type) || "Realisation".Equals(relation.PropertiesEa_type))
            {
                CDClass fromClass = OALProgram.Instance.ExecutionSpace.getClassByName(relation.FromClass);
                CDClass toClass = OALProgram.Instance.ExecutionSpace.getClassByName(relation.ToClass);

                if (fromClass != null && toClass != null)
                    fromClass.SuperClass = toClass;
            }
        }
        return relations;
    }
    private GameObject GenerateRelationTypePrefab(string relationType, string direction)
    {
        switch (relationType)
        {
            case "Association":
                switch (direction)
                {
                    case "Source -> Destination": return ClassDiagram.Instance.associationSDPrefab;
                    case "Destination -> Source": return ClassDiagram.Instance.associationDSPrefab;
                    case "Bi-Directional": return ClassDiagram.Instance.associationFullPrefab;
                    default: return ClassDiagram.Instance.associationNonePrefab;
                }
               
            case "Generalization": return ClassDiagram.Instance.generalizationPrefab;
            case "Dependency": return ClassDiagram.Instance.dependsPrefab;
            case "Realisation": return ClassDiagram.Instance.realisationPrefab;
            default: return ClassDiagram.Instance.associationNonePrefab;
        }
    }
    private void GenerateClassAttributes(Class currentClass, ref CDClass cdcClass)
    {
        foreach (Attribute CurrentAttribute in currentClass.Attributes)
        {
            CurrentAttribute.Name = CurrentAttribute.Name.Replace(" ", "_");
            String AttributeType = EXETypes.ConvertEATypeName(CurrentAttribute.Type);
            if (AttributeType == null)
                continue;
            cdcClass.AddAttribute(new CDAttribute(CurrentAttribute.Name, EXETypes.ConvertEATypeName(AttributeType)));
        }
    }
    private void GenerateClassMethods(Class currentClass, ref CDClass cdcClass)
    {
        foreach (Method CurrentMethod in currentClass.Methods)
        {
            CurrentMethod.Name = CurrentMethod.Name.Replace(" ", "_");
            CDMethod Method = new CDMethod(cdcClass, CurrentMethod.Name, EXETypes.ConvertEATypeName(CurrentMethod.ReturnValue));
            cdcClass.AddMethod(Method);

            foreach (string arg in CurrentMethod.arguments)
            {
                string[] tokens = arg.Split(' ');
                string type = tokens[0];
                string name = tokens[1];

                Method.Parameters.Add(new CDParameter() { Name = name, Type = EXETypes.ConvertEATypeName(type) });
            }
        }
    }
}