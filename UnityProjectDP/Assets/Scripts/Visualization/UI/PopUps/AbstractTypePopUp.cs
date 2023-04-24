using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OALProgramControl;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Visualization.ClassDiagram;
using Visualization.Networking;

namespace Visualization.UI.PopUps
{
    public abstract class AbstractTypePopUp : AbstractClassPopUp
    {
        private const string ErrorTypeEmpty = "Type can not be empty";
        private const string Custom = "custom";
        
        public TMP_Dropdown dropdown;
        public TMP_Text customType;
        public TMP_InputField customTypeField;
        public Toggle isArray;
        private readonly HashSet<TMP_Dropdown.OptionData> _variableData = new();

        protected Transform findAttributeClient(ulong classNetworkId)
        {
            var objects = NetworkManager.Singleton.SpawnManager.SpawnedObjects;
            return objects[classNetworkId]
                .transform
                .Find("Background")
                .Find("Attributes")
                .Find("AttributeLayoutGroup")
                .Find(inp.text);
        }

        private IEnumerable<string> clientClassList()
        {
            var classes = new List<string>();
            var spawnedObjects = NetworkManager.Singleton.SpawnManager.SpawnedObjects;
            foreach (var spawnedObject in spawnedObjects)
            {
                var netClass = spawnedObject.Value.GetComponent<NetworkClass>();
                if (netClass)
                    classes.Add(netClass.name);
            }

            return classes;
        }


        protected new void Awake()
        {
            base.Awake();
            
            dropdown.onValueChanged.AddListener(delegate
            {
                if (dropdown.options[dropdown.value].text == Custom)
                {
                    customType.transform.gameObject.SetActive(true);
                    customTypeField.transform.gameObject.SetActive(true);
                }
                else
                {
                    customType.transform.gameObject.SetActive(false);
                    customTypeField.transform.gameObject.SetActive(false);
                    customTypeField.text = "";
                }
            });
            
            customTypeField.onValueChanged.AddListener(delegate(string arg)
            {
                if (string.IsNullOrEmpty(arg))
                    return;
                if (arg.Length == 1 && (char.IsLetter(arg[0]) || arg[0] == '_'))
                    customTypeField.text = arg;
                else if (arg.Length > 1 && char.IsLetterOrDigit(arg[^1]) || arg[^1] == '_')
                    customTypeField.text = arg;
                else
                    customTypeField.text = arg[..^1];
            });
        }

        protected void SetType(string attributeType)
        {
            var formerArray = attributeType.Contains("[]");
            attributeType = Regex.Replace(attributeType, "[\\[\\]\\n]", "");
            isArray.isOn = formerArray;

            var typeIndex = dropdown.options.FindIndex(x => x.text == attributeType);
            if (typeIndex == -1)
            {
                dropdown.value = dropdown.options.FindIndex(x => x.text == Custom);
                customTypeField.text = attributeType;
            }
            else
            {
                dropdown.value = typeIndex;
            }
        }

        protected new string GetType()
        {
            if (dropdown.options[dropdown.value].text != Custom)
                return (isArray.isOn ? "[]" : "") + dropdown.options[dropdown.value].text;
            if (customTypeField.text.Length == 0)
                DisplayError(ErrorTypeEmpty);

            if (isArray.isOn && customTypeField.text == "void")
                isArray.isOn = false;

            return (isArray.isOn ? "[]" : "") + EXETypes.ConvertEATypeName(customTypeField.text.Replace(" ", "_"));
        }

        private void UpdateDropdown()
        {
            IEnumerable<string> classNames = new List<string>();
            if (isNetworkDisabledOrIsServer())
                classNames = DiagramPool.Instance.ClassDiagram.GetClassList().Select(x => x.Name);
            else
                classNames = clientClassList();

            dropdown.options.RemoveAll(x => _variableData.Contains(x));
            _variableData.Clear();
            _variableData.UnionWith(classNames.Select(x => new TMP_Dropdown.OptionData(x)));
            dropdown.options.AddRange(_variableData);
        }

        public override void ActivateCreation()
        {
            base.ActivateCreation();
            UpdateDropdown();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            dropdown.value = 0;
            customTypeField.text = "";
            isArray.isOn = false;
        }
    }
}
