using TMPro;

namespace Visualization.UI.PopUps
{
    public class AddParameterPopUp : AbstractTypePopUp
    {
        private const string ErrorParameterNameExists = "Parameter with the same name already exists";
        
        public TMP_Text confirm;

        public override void Confirmation()
        {
            if (inp.text == "")
            {
                DisplayError(ErrorEmptyName);
                return;
            }
            
            var parameter = GetType() + " " + inp.text.Replace(" ", "_");
            if (UIEditorManager.Instance.addMethodPopUp.ArgExists(parameter))
            {
                DisplayError(ErrorParameterNameExists);
                return;
            }
            if (_callee == "Add")
                UIEditorManager.Instance.addMethodPopUp.AddArg(parameter);
            else
                UIEditorManager.Instance.editMethodPopUp.AddArg(parameter);
            Deactivate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            UIEditorManager.Instance.editMethodPopUp.gameObject.SetActive(true);
        }
    }
}
