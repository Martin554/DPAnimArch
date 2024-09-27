using System;
using TMPro;
using Unity.Netcode;
using Visualization.ClassDiagram;
using Visualization.ClassDiagram.ClassComponents;

namespace Visualization.UI.PopUps
{
    public class RenameClassPopUp : AbstractClassPopUp
    {
        private const string ErrorClassNameExists = "Class with the same name already exists";
        private Class _formerClass;

        public override void ActivateCreation(TMP_Text classTxt)
        {
            base.ActivateCreation(classTxt);
            inp.text = classTxt.text;
            if (UIEditorManager.Instance.isNetworkDisabledOrIsServer())
            {
                _formerClass = DiagramPool.Instance.ClassDiagram.FindClassByName(inp.text).ParsedClass;
            }
            else
            {
                _networkClassId = findClassClient(inp.text);
            }
        }

        public override void Confirmation()
        {
            if (inp.text == "")
            {
                DisplayError(ErrorEmptyName);
                return;
            }

            var newClassName = inp.text.Replace(" ", "_");
            if (UIEditorManager.Instance.isNetworkDisabledOrIsServer())
            {
                var classInDiagram = DiagramPool.Instance.ClassDiagram.FindClassByName(newClassName);
                if (classInDiagram != null && !_formerClass.Equals(classInDiagram.ParsedClass))
                {
                    DisplayError(ErrorClassNameExists);
                    return;
                }
            }
            else
            {
                if (findClassClient(newClassName) != 0)
                {
                    DisplayError(ErrorClassNameExists);
                    return;
                }
            }

            UIEditorManager.Instance.mainEditor.UpdateNodeName(className.text, newClassName);
            Deactivate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            _formerClass = null;
        }
    }
}
