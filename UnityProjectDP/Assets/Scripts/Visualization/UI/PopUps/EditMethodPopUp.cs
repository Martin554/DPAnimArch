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
    public class EditMethodPopUp : AbstractTypePopUp
    {
        private const string ErrorMethodNameExists = "Method with the same name already exists";
        private const string Void = "void";

        public TMP_Text confirm;
        [SerializeField] private Transform parameterContent;
        private Method _formerMethod;
        private List<string> _parameters = new();
        public TMP_Text options;
        public TMP_Text isArrayText;

        private new void Awake()
        {
            base.Awake();
            dropdown.onValueChanged.AddListener(delegate
            {
                if (dropdown.options[dropdown.value].text == Void)
                {
                    options.transform.gameObject.SetActive(false);
                    isArray.transform.gameObject.SetActive(false);
                    isArrayText.transform.gameObject.SetActive(false);
                }
                else
                {
                    options.transform.gameObject.SetActive(true);
                    isArray.transform.gameObject.SetActive(true);
                    isArrayText.transform.gameObject.SetActive(true);
                }
            });
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

        public void ActivateCreation(TMP_Text classTxt, TMP_Text methodTxt)
        {
            base.ActivateCreation(classTxt);
            var formerMethodName = GetMethodNameFromString(methodTxt.text);
            if (isNetworkDisabledOrIsServer())
            {
                _formerMethod = DiagramPool.Instance.ClassDiagram.FindMethodByName(className.text, formerMethodName);
            }
            else
            {
                _formerMethod = new Method()
                {
                    Name = formerMethodName,
                    ReturnValue = GetMethodTypeFromString(methodTxt.text)
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

            if (isNetworkDisabledOrIsServer())
            {
                var methodInDiagram = DiagramPool.Instance.ClassDiagram.FindMethodByName(className.text, newMethod.Name);
                if (methodInDiagram != null && !_formerMethod.Equals(methodInDiagram))
                {
                    DisplayError(ErrorMethodNameExists);
                    return;
                }

                newMethod.Id = _formerMethod.Id;
            }
            else
            {
                var networkClassId = findClassClient(className.text);
                if (findMethodClient(networkClassId) != null)
                {
                    DisplayError(ErrorMethodNameExists);
                    return;
                }
            }
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

        public bool ArgExists(string parameter)
        {
            return _parameters.Any(x => x == parameter);
        }

        public void AddArg(string parameter)
        {
            _parameters.Add(parameter);
            var instance = Instantiate(DiagramPool.Instance.parameterMethodPrefab, parameterContent, false);
            instance.name = parameter;
            instance.transform.Find("ParameterText").GetComponent<TextMeshProUGUI>().text += parameter;
        }

        public void EditArg(string formerParam, string newParam)
        {
            var index = _parameters.FindIndex(x => x == formerParam);
            _parameters[index] = newParam;
            parameterContent.GetComponentsInChildren<ParameterManager>()
                .First(x => x.parameterTxt.text == formerParam).parameterTxt.text = newParam;
        }

        public void RemoveArg(string parameter)
        {
            _parameters.RemoveAll(x => Equals(x, parameter));
            Destroy(parameterContent.Find(parameter).transform.gameObject);
        }
    }
}
