using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AttributeMenu : MonoBehaviour
{
    public GameObject AtrPanel;
    public TMP_InputField inp;
    public TMP_Dropdown dropdown;
    private TMP_Text attributeText;
    private TMP_Text classText;
    private AttributeModel attribute;
    public Toggle isArray;
    public void ActivateCreation(TMP_Text classText, TMP_Text attributeText)
    {
        AtrPanel.SetActive(true);
        this.attributeText = attributeText;
        this.classText = classText;
        attribute = new AttributeModel();

    }
    public void SetName(string atrName)
    {
        attribute.Name = atrName;
    }
    public void SetType(string type)
    {
        attribute.Type = type;
    }
    public void SaveAtr()
    {
        SetName(inp.text);
        SetType(dropdown.options[dropdown.value].text);
        if (ClassDiagramView.Instance.AddAttribute(classText.text, attribute))
        {
            if (isArray.isOn)
            {
                attributeText.text += attribute.Name + "[]: " + attribute.Type + "\n";
            }
            else
            {
                attributeText.text += attribute.Name + ": " + attribute.Type + "\n";
            }
        }
        attribute = new AttributeModel();
        AtrPanel.SetActive(false);
        inp.text = "";
        isArray.isOn = false;
    }
}
