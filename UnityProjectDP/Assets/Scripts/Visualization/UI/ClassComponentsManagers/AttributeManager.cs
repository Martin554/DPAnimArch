﻿using TMPro;
using UnityEngine;

namespace Visualization.UI.ClassComponentsManagers
{
    public class AttributeManager : MonoBehaviour
    {
        public TMP_Text classTxt;
        public TMP_Text attributeTxt;

        public void OpenAttributeEditPopUp()
        {
            UIEditorManager.Instance.renameAttributePopUp.ActivateCreation(classTxt, attributeTxt);
        }

        public void DeleteAttribute()
        {
            UIEditorManager.Instance.confirmPopUp.ActivateCreation(delegate
            {
                UIEditorManager.Instance.mainEditor.DeleteAttribute(classTxt.text, name);
            });
        }
    }
}
