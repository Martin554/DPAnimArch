using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Visualization.ClassDiagram;
using Visualization.UI.ClassComponentsManagers;

namespace Visualization.UI.PopUps
{
    public class AbstractMethodPopUp : AbstractTypePopUp
    {
        public TMP_Text confirm;
        public TMP_Text options;
        public TMP_Text isArrayText;

        protected const string Void = "void";

        [SerializeField] protected Transform parameterContent;
        protected List<string> _parameters = new();

        private new void Awake()
        {
            base.Awake();
            dropdown.onValueChanged.AddListener(delegate
            {
                if (dropdown.options[dropdown.value].text == Void)
                {
                    options.transform.gameObject.SetActive(false);
                    isArray.transform.gameObject.SetActive(false);
                    isArrayText.transform.gameObject.SetActive(false);
                }
                else
                {
                    options.transform.gameObject.SetActive(true);
                    isArray.transform.gameObject.SetActive(true);
                    isArrayText.transform.gameObject.SetActive(true);
                }
            });
        }

        public bool ArgExists(string parameter)
        {
            return _parameters.Any(x => x == parameter);
        }

        public void AddArg(string parameter)
        {
            _parameters.Add(parameter);
            var instance = Instantiate(DiagramPool.Instance.parameterMethodPrefab, parameterContent, false);
            instance.name = parameter;
            instance.transform.Find("ParameterText").GetComponent<TextMeshProUGUI>().text += parameter;
        }

        public void EditArg(string formerParam, string newParam)
        {
            var index = _parameters.FindIndex(x => x == formerParam);
            _parameters[index] = newParam;
            parameterContent.GetComponentsInChildren<ParameterManager>()
                .First(x => x.parameterTxt.text == formerParam).parameterTxt.text = newParam;
        }

        public void RemoveArg(string parameter)
        {
            _parameters.RemoveAll(x => Equals(x, parameter));
            Destroy(parameterContent.Find(parameter).transform.gameObject);
        }
    }
}
