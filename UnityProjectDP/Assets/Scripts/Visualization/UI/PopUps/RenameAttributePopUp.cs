using System;
using TMPro;
using Visualization.ClassDiagram;
using Attribute = Visualization.ClassDiagram.ClassComponents.Attribute;

namespace Visualization.UI.PopUps
{
    public class RenameAttributePopUp : AbstractTypePopUp
    {
        private const string ErrorAttributeNameExists = "Attribute with the same name already exists";
        
        public TMP_Text confirm;
        private Attribute _formerAttribute;
        private string _formerName;

        public void ActivateCreation(TMP_Text classTxt, TMP_Text attributeTxt)
        {
            base.ActivateCreation(classTxt);
            var text = attributeTxt.text.Split(": ");
            var formerName = text[0];

            var attributeType = text[1];
            SetType(attributeType);

            _formerName = formerName;
            inp.text = formerName;
            _formerAttribute = DiagramPool.Instance.ClassDiagram.FindAttributeByName(className.text, formerName);
        }

        public override void Confirmation()
        {
            if (inp.text == "")
            {
                DisplayError(ErrorEmptyName);
                return;
            }
            var newAttribute = new Attribute();
            if (isNetworkDisabledOrIsServer())
            {
                var attributeInDiagram = DiagramPool.Instance.ClassDiagram.FindAttributeByName(className.text, inp.text);
                if (attributeInDiagram != null && !_formerAttribute.Equals(attributeInDiagram))
                {
                    DisplayError(ErrorAttributeNameExists);
                    return;
                }
                newAttribute.Id = _formerAttribute.Id;
            }
            else
            {
                var networkClassId = findClassClient(className.text);
                if (findAttributeClient(networkClassId) != null)
                {
                    DisplayError(ErrorAttributeNameExists);
                    return;
                }
            }

            newAttribute.Name = inp.text;
            newAttribute.Type = GetType();

            UIEditorManager.Instance.mainEditor.UpdateAttribute(className.text, _formerName, newAttribute);

            Deactivate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            _formerAttribute = null;
            _formerName = "";
        }
    }
}
