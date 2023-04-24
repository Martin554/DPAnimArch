using System;
using TMPro;
using Unity.Netcode;
using Visualization.ClassDiagram;
using Visualization.ClassDiagram.ClassComponents;

namespace Visualization.UI.PopUps
{
    public class ClassPopUp : AbstractClassPopUp
    {
        private const string ErrorClassNameExists = "Class with the same name already exists";
        public TMP_Text confirm;
        private Class _formerClass;
        private ulong _classId;

        public override void ActivateCreation()
        {
            base.ActivateCreation();
            confirm.text = "Add";
            _classId = 0;
        }

        private void findClassClient()
        {
            var objects = NetworkManager.Singleton.SpawnManager.SpawnedObjects;
            var values = objects.Values;
            foreach (var value in values)
            {
                if (value.name == inp.text)
                {
                    _classId = value.NetworkObjectId;
                    return;
                }
            }
            _classId = 0;
        }

        private void CreateClass(string className)
        {
            var newClass = new Class(className, Guid.NewGuid().ToString());

            if (_classId != 0)
            {
                DisplayError(ErrorClassNameExists);
                return;
            }

            UIEditorManager.Instance.mainEditor.CreateNode(newClass);
        }

        private void RenameClass(string newClassName)
        {
            if (UIEditorManager.Instance.NetworkEnabled)
            {
                if (NetworkManager.Singleton.IsServer)
                {
                    var classInDiagram = DiagramPool.Instance.ClassDiagram.FindClassByName(newClassName);

                    if (_classId != 0 && !_formerClass.Equals(classInDiagram.ParsedClass))
                    {
                        _formerClass = DiagramPool.Instance.ClassDiagram.FindClassByName(inp.text).ParsedClass;
                        confirm.text = "Edit";
                    }
                }
                else
                {
                    if (_classId == 0)
                    {
                        DisplayError(ErrorClassNameExists);
                        return;
                    }
                }
            }
            else if (_classId != 0)
            {
                var classInDiagram = DiagramPool.Instance.ClassDiagram.FindClassByName(newClassName);
                if (!_formerClass.Equals(classInDiagram.ParsedClass))
                {
                    DisplayError(ErrorClassNameExists);
                    return;
                }
            }

            UIEditorManager.Instance.mainEditor.UpdateNodeName(className.text, newClassName);
        }

        public override void ActivateCreation(TMP_Text classTxt)
        {
            base.ActivateCreation(classTxt);
            inp.text = classTxt.text;
            if (UIEditorManager.Instance.NetworkEnabled)
            {
                if (NetworkManager.Singleton.IsServer)
                {
                    _formerClass = DiagramPool.Instance.ClassDiagram.FindClassByName(inp.text).ParsedClass;
                    confirm.text = "Edit";
                }
                else
                {
                    findClassClient();
                    if (_classId != 0)
                    {
                        confirm.text = "Edit";
                    }
                }
            }
            else
            {
                _formerClass = DiagramPool.Instance.ClassDiagram.FindClassByName(inp.text).ParsedClass;
                confirm.text = "Edit";
            }
        }

        public override void Confirmation()
        {
            if (inp.text == "")
            {
                DisplayError(ErrorEmptyName);
                return;
            }

            var inpClassName = inp.text.Replace(" ", "_");
            if (_formerClass == null && _classId == 0)
            {
                CreateClass(inpClassName);
            }
            else
            {
                RenameClass(inpClassName);
            }

            Deactivate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            _formerClass = null;
            _classId = 0;
        }
    }
}
