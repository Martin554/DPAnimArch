﻿using UnityEngine;
using TMPro;
using AnimArch.Visualization.Diagrams;

namespace AnimArch.Visualization.UI
{
    public class PopUpManager : MonoBehaviour
    {
        public TMP_Text classTxt;

        public void OpenAttributePopUp()
        {
            ClassEditor.Instance.attributePopUp.ActivateCreation(classTxt);
        }

        public void OpenMethodPopUp()
        {
            ClassEditor.Instance.methodPopUp.ActivateCreation(classTxt);
        }

        public void OpenClassPopUp()
        {
            ClassEditor.Instance.classPopUp.ActivateCreation(classTxt);
        }
    }
}