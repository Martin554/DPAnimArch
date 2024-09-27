using TMPro;
using UnityEngine;

namespace Visualization.UI.ClassComponentsManagers
{
    public class ParameterManager : MonoBehaviour
    {
        public TMP_Text parameterTxt;

        public void OpenParameterEditPopUp()
        {
            UIEditorManager.Instance.editMethodPopUp.panel.SetActive(false);
            UIEditorManager.Instance.editParameterPopUp.ActivateCreation(parameterTxt);
        }

        public void DeleteParameter()
        {
            UIEditorManager.Instance.editMethodPopUp.panel.SetActive(false);
            UIEditorManager.Instance.confirmPopUp.ActivateCreation(delegate
            {
                UIEditorManager.Instance.editMethodPopUp.RemoveArg(name);
                UIEditorManager.Instance.editMethodPopUp.panel.SetActive(true);
            });

            UIEditorManager.Instance.confirmPopUp.cancelButton.onClick.AddListener(delegate
            {
                UIEditorManager.Instance.editMethodPopUp.panel.SetActive(true);
            });

            UIEditorManager.Instance.confirmPopUp.exitButton.onClick.AddListener(delegate
            {
                UIEditorManager.Instance.editMethodPopUp.panel.SetActive(true);
            });
        }
    }
}
