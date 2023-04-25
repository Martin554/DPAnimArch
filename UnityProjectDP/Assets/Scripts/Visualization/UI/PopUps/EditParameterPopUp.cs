using TMPro;

namespace Visualization.UI.PopUps
{
    public class EditParameterPopUp : AbstractTypePopUp
    {
        private const string ErrorParameterNameExists = "Parameter with the same name already exists";

        public TMP_Text confirm;
        private string _formerParam;

        public override void ActivateCreation(TMP_Text parameterTxt)
        {
            ActivateCreation();
            var par = parameterTxt.text.Split(" ");
            inp.text = par[1];

            SetType(par[0]);
            _formerParam = parameterTxt.text;
        }

        public override void Confirmation()
        {
            if (inp.text == "")
            {
                DisplayError(ErrorEmptyName);
                return;
            }

            var parameter = GetType() + " " + inp.text.Replace(" ", "_");
            if (UIEditorManager.Instance.editMethodPopUp.ArgExists(parameter))
            {
                DisplayError(ErrorParameterNameExists);
                return;
            }

            if (_callee == "Add")
                UIEditorManager.Instance.addMethodPopUp.EditArg(_formerParam, parameter);
            else
                UIEditorManager.Instance.editMethodPopUp.EditArg(_formerParam, parameter);
            Deactivate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            UIEditorManager.Instance.editMethodPopUp.gameObject.SetActive(true);
            _formerParam = null;
        }
    }
}
