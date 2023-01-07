﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using AnimArch.Visualization.Diagrams;
using Microsoft.Msagl.Core.DataStructures;

namespace AnimArch.Visualization.UI
{
    public class MethodPopUp : AbstractPopUp
    {
        public TMP_Dropdown dropdown;
        public TMP_Text confirm;
        [SerializeField] Transform parameterContent;
        private List<string> _parameters = new();
        private readonly HashSet<TMP_Dropdown.OptionData> _variableData = new();
        private string _formerName;
        
        private void UpdateDropdown()
        {
            var classNames = DiagramPool.Instance.ClassDiagram.GetClassList().Select(x => x.Name);
            
            dropdown.options.RemoveAll(x => _variableData.Contains(x));
            _variableData.Clear();
            _variableData.UnionWith(classNames.Select(x => new TMP_Dropdown.OptionData(x)));
            dropdown.options.AddRange(_variableData);
        }


        public override void ActivateCreation(TMP_Text classTxt)
        {
            base.ActivateCreation(classTxt);
            UpdateDropdown();
            confirm.text = "Add";
        }
        
        
        public void ActivateCreation(TMP_Text classTxt, TMP_Text methodTxt)
        {
            ActivateCreation(classTxt);

            var formerMethod = ClassEditor.GetMethodFromString(methodTxt.text);
            inp.text = formerMethod.Name;
            
            dropdown.value = dropdown.options.FindIndex(x => x.text == formerMethod.ReturnValue);
            _parameters = formerMethod.arguments;
            parameterContent.GetComponentInChildren<TMP_Text>().text = _parameters.Aggregate((a, b) => a + ",\n" + b);
            _formerName = formerMethod.Name;
            confirm.text = "Edit";
        }


        public override void Confirmation()
        {
            if (inp.text == "")
            {
                Deactivate();
                return;
            }

            var newMethod = new Method
            {
                Name = inp.text,
                ReturnValue = dropdown.options[dropdown.value].text,
                arguments = _parameters
            };
            if (_formerName == null)
                ClassEditor.AddMethod(className.text, newMethod, ClassEditor.Source.editor);
            else
            {
                ClassEditor.UpdateMethod(className.text, _formerName, newMethod);
                _formerName = null;
            }
            
            Deactivate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            _parameters = new List<string>();
            parameterContent.GetComponentInChildren<TMP_Text>().text = "";
            dropdown.value = 0;
        }

        public void AddArg(string parameter)
        {
            _parameters.Add(parameter);
            parameterContent.GetComponentInChildren<TMP_Text>().text =
                parameterContent.GetComponentInChildren<TMP_Text>().text == ""
                    ? parameter
                    : parameterContent.GetComponentInChildren<TMP_Text>().text + ",\n" + parameter;
        }
    }
}