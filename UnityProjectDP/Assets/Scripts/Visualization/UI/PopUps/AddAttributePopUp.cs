using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Visualization.ClassDiagram;
using Attribute = Visualization.ClassDiagram.ClassComponents.Attribute;

namespace Visualization.UI.PopUps
{
    public class AddAttributePopUp : AbstractTypePopUp
    {
        private const string ErrorAttributeNameExists = "Attribute with the same name already exists";
        public TMP_Text confirm;

        public override void Confirmation()
        {
            if (inp.text == "")
            {
                DisplayError(ErrorEmptyName);
                return;
            }

            if (isNetworkDisabledOrIsServer())
            {
                if (DiagramPool.Instance.ClassDiagram.FindAttributeByName(className.text, inp.text) != null)
                {
                    DisplayError(ErrorAttributeNameExists);
                    return;
                }
            }
            else
            {
                var classNetworkId = findClassClient(className.text);
                if (classNetworkId == 0)
                {
                    DisplayError(ErrorEmptyName);
                    return;
                }

                if (findAttributeClient(classNetworkId) != null)
                {
                    DisplayError(ErrorAttributeNameExists);
                    return;
                }
            }

            var newAttribute = new Attribute
            {
                Id = Guid.NewGuid().ToString(),
                Name = inp.text,
                Type = GetType()
            };

            UIEditorManager.Instance.mainEditor.AddAttribute(className.text, newAttribute);
            Deactivate();
        }
    }
}
