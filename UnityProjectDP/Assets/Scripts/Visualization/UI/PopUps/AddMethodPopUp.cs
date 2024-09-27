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

        private new void Awake()
        {
            base.Awake();
        }

        public override void ActivateCreation()
        {
            base.ActivateCreation();
            UIEditorManager.Instance.ParameterPopUpCallee = "Add";
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

                if (findMethodClient(classNetworkId, newMethod.Name) != null)
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
