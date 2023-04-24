using System;
using TMPro;
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
            _formerClass = DiagramPool.Instance.ClassDiagram.FindClassByName(inp.text).ParsedClass;
        }

        public override void Confirmation()
        {
            if (inp.text == "")
            {
                DisplayError(ErrorEmptyName);
                return;
            }

            var newClassName = inp.text.Replace(" ", "_");
            var classInDiagram = DiagramPool.Instance.ClassDiagram.FindClassByName(newClassName);
            if (classInDiagram != null && !_formerClass.Equals(classInDiagram.ParsedClass))
            {
                DisplayError(ErrorClassNameExists);
                return;
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
