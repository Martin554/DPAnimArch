using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Visualization.ClassDiagram;
using Visualization.ClassDiagram.ClassComponents;
using Visualization.UI.ClassComponentsManagers;

namespace Visualization.UI.PopUps
{
    public class EditMethodPopUp : AbstractMethodPopUp
    {
        private Method _formerMethod;

        private new void Awake()
        {
            base.Awake();
        }

        private static string GetMethodNameFromString(string str)
        {
            var parts = str.Split(new[] { ": ", "\n" }, StringSplitOptions.None);

            var nameAndArguments = parts[0].Split(new[] { "(", ")" }, StringSplitOptions.None);
            return nameAndArguments[0];
        }

        private static string GetMethodTypeFromString(string str)
        {
            var parts = str.Split(new[] { ": ", "\n" }, StringSplitOptions.None);
            return parts[1];
        }

        private static List<string> GetArgumentsFromString(string str)
        {
            var parts = str.Split(new[] { ": ", "\n" }, StringSplitOptions.None);
            var nameAndArguments = parts[0].Split(new[] { "(", ")" }, StringSplitOptions.None);
            if (nameAndArguments.Length > 1 && nameAndArguments[1] != "")
            {
                nameAndArguments[1].Replace(" ", "");
                var arguments = nameAndArguments[1].Split(new[] { "," }, StringSplitOptions.None);
                return arguments.ToList();
            }
            return new List<string>();
        }

        public void ActivateCreation(TMP_Text classTxt, TMP_Text methodTxt)
        {
            base.ActivateCreation(classTxt);
            UIEditorManager.Instance.ParameterPopUpCallee = "Edit";
            var formerMethodName = GetMethodNameFromString(methodTxt.text);
            if (UIEditorManager.Instance.isNetworkDisabledOrIsServer())
            {
                _formerMethod = DiagramPool.Instance.ClassDiagram.FindMethodByName(className.text, formerMethodName);
            }
            else
            {
                _formerMethod = new Method()
                {
                    Name = formerMethodName,
                    ReturnValue = GetMethodTypeFromString(methodTxt.text),
                    arguments = GetArgumentsFromString(methodTxt.text)
                };
            }

            inp.text = _formerMethod.Name;
            SetType(_formerMethod.ReturnValue);
            _formerMethod.arguments.ForEach(AddArg);
        }

        public override void Confirmation()
        {
            if (inp.text == "")
            {
                DisplayError(ErrorEmptyName);
                return;
            }

            var newMethod = new Method
            {
                Name = inp.text,
                ReturnValue = GetType(),
                arguments = _parameters
            };

            if (UIEditorManager.Instance.isNetworkDisabledOrIsServer())
                newMethod.Id = _formerMethod.Id;

            UIEditorManager.Instance.mainEditor.UpdateMethod(className.text, _formerMethod.Name, newMethod);
            Deactivate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            _parameters = new List<string>();
            parameterContent.DetachChildren();
            _formerMethod = null;
        }
    }
}
