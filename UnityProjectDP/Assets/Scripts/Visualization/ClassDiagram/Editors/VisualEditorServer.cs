using AnimArch.Extensions;
using AnimArch.Visualization.UI;
using Networking;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace AnimArch.Visualization.Diagrams
{
    public class VisualEditorServer : VisualEditor
    {
        private Transform GetGraphUnits()
        {
            var graphTransform = DiagramPool.Instance.ClassDiagram.graph.gameObject.GetComponent<Transform>();
            var graphUnits = graphTransform.Find("Units");
            return graphUnits.GetComponent<Transform>();
        }
        public override GameObject CreateNode(Class newClass)
        {
            var nodeGo = DiagramPool.Instance.ClassDiagram.graph.AddNode();
            nodeGo.name = newClass.Name;

            SetDefaultPosition(nodeGo);
            if (!UIEditorManager.Instance.NetworkEnabled)
            {
                var graphTransform = DiagramPool.Instance.ClassDiagram.graph.gameObject.GetComponent<Transform>();
                var graphUnits = graphTransform.Find("Units");
                nodeGo.GetComponent<Transform>().SetParent(graphUnits.GetComponent<Transform>());
            }

            var nodeNo = nodeGo.GetComponent<NetworkObject>();
            nodeNo.Spawn();
            Spawner.Instance.SetNetworkObjectNameClientRpc(nodeNo.name, nodeNo.NetworkObjectId);

            if (!nodeNo.TrySetParent(GetGraphUnits(), false))
            {
                throw new InvalidParentException(nodeNo.name);
            }
            Spawner.Instance.SetClassNameClientRpc(nodeNo.name, nodeNo.NetworkObjectId);

            UpdateNodeName(nodeGo);

            return nodeGo;
        }

        public override void UpdateNodeName(GameObject classGo)
        {
            base.UpdateNodeName(classGo);
            var networkId = classGo.GetComponent<NetworkObject>().NetworkObjectId;
            Spawner.Instance.SetClassNameClientRpc(classGo.name, networkId);
        }

        public override void AddAttribute(ClassInDiagram classInDiagram, Attribute attribute)
        {
            base.AddAttribute(classInDiagram, attribute);

            var classNo = classInDiagram.VisualObject.GetComponent<NetworkObject>();
            Spawner.Instance.AddAttributeClientRpc(attribute.Name, GetStringFromAttribute(attribute), classNo.NetworkObjectId);
        }

        public override void UpdateAttribute(ClassInDiagram classInDiagram, string oldAttribute, Attribute newAttribute)
        {
            base.UpdateAttribute(classInDiagram, oldAttribute, newAttribute);
            var classNo = classInDiagram.VisualObject.GetComponent<NetworkObject>();
            Spawner.Instance.UpdateAttributeClientRpc(oldAttribute, newAttribute.Name, GetStringFromAttribute(newAttribute), classNo.NetworkObjectId);
        }
        public override void DeleteAttribute(ClassInDiagram classInDiagram, string attribute)
        {
            base.DeleteAttribute(classInDiagram, attribute);
            var classNo = classInDiagram.VisualObject.GetComponent<NetworkObject>();
            Spawner.Instance.DeleteAttributeClientRpc(attribute, classNo.NetworkObjectId);
        }

        public override void AddMethod(ClassInDiagram classInDiagram, Method method)
        {
            base.AddMethod(classInDiagram, method);
            var classNo = classInDiagram.VisualObject.GetComponent<NetworkObject>();
            Spawner.Instance.AddMethodClientRpc(method.Name, GetStringFromMethod(method), classNo.NetworkObjectId);
        }

        public override void UpdateMethod(ClassInDiagram classInDiagram, string oldMethod, Method newMethod)
        {
            base.UpdateMethod(classInDiagram, oldMethod, newMethod);
            var classNo = classInDiagram.VisualObject.GetComponent<NetworkObject>();
            Spawner.Instance.UpdateMethodClientRpc(oldMethod, newMethod.Name, GetStringFromMethod(newMethod), classNo.NetworkObjectId);
        }

        public override void DeleteMethod(ClassInDiagram classInDiagram, string method)
        {
            Object.Destroy(GetMethodLayoutGroup(classInDiagram.VisualObject).Find(method).transform.gameObject);
            var classNo = classInDiagram.VisualObject.GetComponent<NetworkObject>();
            Spawner.Instance.DeleteMethodClientRpc(method, classNo.NetworkObjectId);
        }

        public override GameObject CreateRelation(Relation relation)
        {
            var prefab = relation.PropertiesEaType switch
            {
                "Association" => relation.PropertiesDirection switch
                {
                    "Source -> Destination" => DiagramPool.Instance.networkAssociationSDPrefab,
                    "Destination -> Source" => DiagramPool.Instance.networkAssociationDSPrefab,
                    "Bi-Directional" => DiagramPool.Instance.networkAssociationFullPrefab,
                    _ => DiagramPool.Instance.networkAssociationNonePrefab
                },
                "Generalization" => DiagramPool.Instance.networkGeneralizationPrefab,
                "Dependency" => DiagramPool.Instance.networkDependsPrefab,
                "Realisation" => DiagramPool.Instance.networkRealisationPrefab,
                _ => DiagramPool.Instance.networkAssociationNonePrefab
            };

            var sourceClassGo = DiagramPool.Instance.ClassDiagram.FindClassByName(relation.FromClass).VisualObject;
            var destinationClassGo = DiagramPool.Instance.ClassDiagram.FindClassByName(relation.ToClass).VisualObject;

            var edge = DiagramPool.Instance.ClassDiagram.graph.AddEdge(sourceClassGo, destinationClassGo, prefab);

            var edgeNo = edge.GetComponent<NetworkObject>();


            if (edge.gameObject.transform.childCount > 0)
                DiagramPool.Instance.ClassDiagram.StartCoroutine(QuickFix(edge.transform.GetChild(0).gameObject));

            edgeNo.Spawn();

            if (!edgeNo.TrySetParent(GetGraphUnits(), false))
            {
                throw new InvalidParentException(edgeNo.name);
            }

            return edge;
        }
    }
}
