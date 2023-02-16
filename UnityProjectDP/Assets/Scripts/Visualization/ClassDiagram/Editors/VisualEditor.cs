﻿using System.Collections;
using AnimArch.Extensions;
using AnimArch.Visualization.UI;
using Networking;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AnimArch.Visualization.Diagrams
{
    public class VisualEditor : IVisualEditor
    {
        public override void UpdateNodeName(GameObject classGo)
        {
            GetNodeHeader(classGo)
                .GetComponent<TextMeshProUGUI>()
                .text = classGo.name;
        }

        public override GameObject CreateNode(Class newClass)
        {
            var nodeGo = DiagramPool.Instance.ClassDiagram.graph.AddNode();
            nodeGo.name = newClass.Name;

            SetDefaultPosition(nodeGo);
            UpdateNodeName(nodeGo);
            if (!UIEditorManager.Instance.NetworkEnabled)
            {
                var graphTransform = DiagramPool.Instance.ClassDiagram.graph.gameObject.GetComponent<Transform>();
                var graphUnits = graphTransform.Find("Units");
                nodeGo.GetComponent<Transform>().SetParent(graphUnits.GetComponent<Transform>());
            }
            return nodeGo;
        }

        public override void SetPosition(string className, Vector3 position, bool fromRpc)
        {
            var classInDiagram = DiagramPool.Instance.ClassDiagram.FindClassByName(className);
            if (classInDiagram != null)
                classInDiagram
                    .VisualObject
                    .GetComponent<RectTransform>()
                    .position = position;
        }

        public override void UpdateNode(GameObject classGo)
        {
            UpdateNodeName(classGo);

            foreach (var attribute in GetAttributeLayoutGroup(classGo).GetComponents<AttributeManager>())
                attribute.classTxt = GetNodeHeader(classGo).GetComponent<TextMeshProUGUI>();

            foreach (var method in GetMethodLayoutGroup(classGo).GetComponents<MethodManager>())
                method.classTxt = GetNodeHeader(classGo).GetComponent<TextMeshProUGUI>();
        }

        protected string GetStringFromAttribute(Attribute attribute)
        {
            return attribute.Name + ": " + attribute.Type;
        }

        public override void AddAttribute(ClassInDiagram classInDiagram, Attribute attribute)
        {
            var attributeLayoutGroup = GetAttributeLayoutGroup(classInDiagram.VisualObject);
            var instance = Object.Instantiate(DiagramPool.Instance.classAttributePrefab, attributeLayoutGroup, false);

            instance.name = attribute.Name;
            instance.transform.Find("AttributeText").GetComponent<TextMeshProUGUI>().text +=
                GetStringFromAttribute(attribute);
            instance.GetComponent<AttributeManager>().classTxt =
                GetNodeHeader(classInDiagram.VisualObject).GetComponent<TextMeshProUGUI>();

            if (UIEditorManager.Instance.active)
                instance.GetComponentsInChildren<Button>(true)
                    .ForEach(x => x.gameObject.SetActive(true));
        }

        public override void UpdateAttribute(ClassInDiagram classInDiagram, string oldAttribute, Attribute newAttribute)
        {
            var attribute = GetAttributeLayoutGroup(classInDiagram.VisualObject).Find(oldAttribute);

            attribute.name = newAttribute.Name;
            attribute.Find("AttributeText").GetComponent<TextMeshProUGUI>().text = GetStringFromAttribute(newAttribute);
        }

        protected string GetStringFromMethod(Method method)
        {
            var arguments = "(";
            if (method.arguments != null)
                for (var index = 0; index < method.arguments.Count; index++)
                    if (index < method.arguments.Count - 1)
                        arguments += method.arguments[index] + ", ";
                    else arguments += method.arguments[index];

            arguments += "): ";

            return method.Name + arguments + method.ReturnValue;
        }

        public override void AddMethod(ClassInDiagram classInDiagram, Method method)
        {
            var methodLayoutGroup = GetMethodLayoutGroup(classInDiagram.VisualObject);
            var instance = Object.Instantiate(DiagramPool.Instance.classMethodPrefab, methodLayoutGroup, false);

            instance.name = method.Name;
            instance.transform.Find("MethodText").GetComponent<TextMeshProUGUI>().text += GetStringFromMethod(method);
            instance.GetComponent<MethodManager>().classTxt =
                GetNodeHeader(classInDiagram.VisualObject).GetComponent<TextMeshProUGUI>();

            if (UIEditorManager.Instance.active)
                instance.GetComponentsInChildren<Button>(true)
                    .ForEach(x => x.gameObject.SetActive(true));
        }

        public override void UpdateMethod(ClassInDiagram classInDiagram, string oldMethod, Method newMethod)
        {
            var method = GetMethodLayoutGroup(classInDiagram.VisualObject).Find(oldMethod);

            method.name = newMethod.Name;
            method.Find("MethodText").GetComponent<TextMeshProUGUI>().text = GetStringFromMethod(newMethod);
        }

        public override GameObject CreateRelation(Relation relation)
        {
            var prefab = relation.PropertiesEaType switch
            {
                "Association" => relation.PropertiesDirection switch
                {
                    "Source -> Destination" => DiagramPool.Instance.associationSDPrefab,
                    "Destination -> Source" => DiagramPool.Instance.associationDSPrefab,
                    "Bi-Directional" => DiagramPool.Instance.associationFullPrefab,
                    _ => DiagramPool.Instance.associationNonePrefab
                },
                "Generalization" => DiagramPool.Instance.generalizationPrefab,
                "Dependency" => DiagramPool.Instance.dependsPrefab,
                "Realisation" => DiagramPool.Instance.realisationPrefab,
                _ => DiagramPool.Instance.associationNonePrefab
            };

            var sourceClassGo = DiagramPool.Instance.ClassDiagram.FindClassByName(relation.FromClass).VisualObject;
            var destinationClassGo = DiagramPool.Instance.ClassDiagram.FindClassByName(relation.ToClass).VisualObject;

            var edge = DiagramPool.Instance.ClassDiagram.graph.AddEdge(sourceClassGo, destinationClassGo, prefab);

            if (edge.gameObject.transform.childCount > 0)
                DiagramPool.Instance.ClassDiagram.StartCoroutine(QuickFix(edge.transform.GetChild(0).gameObject));

            return edge;
        }

        public override void DeleteRelation(RelationInDiagram relationInDiagram)
        {
            DiagramPool.Instance.ClassDiagram.graph.RemoveEdge(relationInDiagram.VisualObject);
        }

        public override void DeleteNode(ClassInDiagram classInDiagram)
        {
            DiagramPool.Instance.ClassDiagram.graph.RemoveNode(classInDiagram.VisualObject);
        }
        
        public override void DeleteAttribute(ClassInDiagram classInDiagram, string attribute)
        {
            Object.Destroy(GetAttributeLayoutGroup(classInDiagram.VisualObject).Find(attribute).transform.gameObject);
        }

        public override void DeleteMethod(ClassInDiagram classInDiagram, string method)
        {
            Object.Destroy(GetMethodLayoutGroup(classInDiagram.VisualObject).Find(method).transform.gameObject);
        }

        //Fix used to minimize relation displaying bug
        protected IEnumerator QuickFix(GameObject g)
        {
            yield return new WaitForSeconds(0.05f);
            g.SetActive(false);
            yield return new WaitForSeconds(0.05f);
            g.SetActive(true);
        }

        protected Transform GetNodeHeader(GameObject classGo)
        {
            return classGo.transform.Find("Background").Find("HeaderLayout").Find("Header");
        }

        protected Transform GetAttributeLayoutGroup(GameObject classGo)
        {
            return classGo.transform
                .Find("Background")
                .Find("Attributes")
                .Find("AttributeLayoutGroup");
        }

        protected Transform GetMethodLayoutGroup(GameObject classGo)
        {
            return classGo.transform
                .Find("Background")
                .Find("Methods")
                .Find("MethodLayoutGroup");
        }

        private void SetDefaultPosition(GameObject node)
        {
            var rect = node.GetComponent<RectTransform>();
            rect.position = new Vector3(100f, 200f, 1);
        }
    }
}
