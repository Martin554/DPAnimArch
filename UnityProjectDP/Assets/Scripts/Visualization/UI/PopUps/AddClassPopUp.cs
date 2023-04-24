﻿using System;
using TMPro;
using Visualization.ClassDiagram;
using Visualization.ClassDiagram.ClassComponents;

namespace Visualization.UI.PopUps
{
    public class AddClassPopUp : AbstractClassPopUp
    {
        private const string ErrorClassNameExists = "Class with the same name already exists";
        public TMP_Text confirm;

        public override void Confirmation()
        {
            if (inp.text == "")
            {
                DisplayError(ErrorEmptyName);
                return;
            }

            var inpClassName = inp.text.Replace(" ", "_");
            var newClass = new Class(inpClassName, Guid.NewGuid().ToString());
            if (isNetworkDisabledOrIsServer())
            {
                if (DiagramPool.Instance.ClassDiagram.FindClassByName(newClass.Name) != null)
                {
                    DisplayError(ErrorClassNameExists);
                    return;
                }
            }
            else
            {
                if (findClassClient(inpClassName) != 0)
                {
                    DisplayError(ErrorClassNameExists);
                    return;
                }
            }

            UIEditorManager.Instance.mainEditor.CreateNode(newClass);
            Deactivate();
        }
    }
}
