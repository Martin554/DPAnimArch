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
        private bool _classExists;

        public override void ActivateCreation()
        {
            base.ActivateCreation();
            confirm.text = "Add";
            _classExists = false;
        }

        private ulong findClassClient()
        {
            var objects = NetworkManager.Singleton.SpawnManager.SpawnedObjects;
            var values = objects.Values;
            foreach (var value in values)
            {
                if (value.name == inp.text)
                {
                    _classExists = true;
                    confirm.text = "Edit";
                    return value.NetworkObjectId;
                }
            }
            return 0;
        }

        private void CreateClass(string className)
        {
            var newClass = new Class(className, Guid.NewGuid().ToString());

            if (_classExists)
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

                    if (_classExists && !_formerClass.Equals(classInDiagram.ParsedClass))
                    {
                        _formerClass = DiagramPool.Instance.ClassDiagram.FindClassByName(inp.text).ParsedClass;
                        confirm.text = "Edit";
                    }
                }
                else
                {
                    var no = findClassClient();
                    if (no != 0)
                    {
                        confirm.text = "Edit";
                    }
                    else
                    {
                        DisplayError(ErrorClassNameExists);
                        return;
                    }
                }
            }
            else if (_classExists)
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
                    var no = findClassClient();
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
            if (_formerClass == null && !_classExists)
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
        }
    }
}
