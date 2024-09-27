using TMPro;
using UnityEngine;

namespace Visualization.UI.ClassComponentsManagers
{
    public class ClassManager : MonoBehaviour
    {
        public TMP_Text classTxt;

        public void OpenAddAttributePopUp()
        {
            UIEditorManager.Instance.addAttributePopUp.ActivateCreation(classTxt);
        }
        public void OpenRenameAttributePopUp()
        {
            UIEditorManager.Instance.renameAttributePopUp.ActivateCreation(classTxt);
        }

        public void OpenAddMethodPopUp()
        {
            UIEditorManager.Instance.addMethodPopUp.ActivateCreation(classTxt);
        }

        public void OpenEditMethodPopUp()
        {
            UIEditorManager.Instance.editMethodPopUp.ActivateCreation(classTxt);
        }

        public void OpenAddClassPopUp()
        {
            UIEditorManager.Instance.addClassPopUp.ActivateCreation(classTxt);
        }
        public void OpenRenameClassPopUp()
        {
            UIEditorManager.Instance.renameClassPopUp.ActivateCreation(classTxt);
        }

        public void DeleteClass()
        {
            UIEditorManager.Instance.confirmPopUp.ActivateCreation(delegate
            {
                UIEditorManager.Instance.mainEditor.DeleteNode(name);
            });
        }
    }
}
