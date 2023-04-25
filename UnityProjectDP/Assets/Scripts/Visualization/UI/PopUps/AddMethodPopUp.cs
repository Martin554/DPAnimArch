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
    public class AddMethodPopUp : AbstractMethodPopUp
    {
        private const string ErrorMethodNameExists = "Method with the same name already exists";
        private const string Void = "void";

        public TMP_Text confirm;
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
                if (DiagramPool.Instance.ClassDiagram.FindMethodByName(className.text, newMethod.Name) != null)
                {
                    DisplayError(ErrorMethodNameExists);
                    return;
                }
                newMethod.Id = Guid.NewGuid().ToString();
            }
            else
            {
                var classNetworkId = findClassClient(className.text);
                if (classNetworkId == 0)
                {
                    DisplayError(ErrorEmptyName);
                    return;
                }

                if (findMethodClient(classNetworkId) != null)
                {
                    DisplayError(ErrorMethodNameExists);
                    return;
                }
            }

            UIEditorManager.Instance.mainEditor.AddMethod(className.text, newMethod);
            Deactivate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            _parameters = new List<string>();
            parameterContent.DetachChildren();
        }
    }
}
