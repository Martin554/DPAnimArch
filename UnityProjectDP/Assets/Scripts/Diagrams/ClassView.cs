using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClassView
{
    private GameObject gameObject;
    public ulong Id { get; set; }
    public float Left { get; set; }
    public float Right { get; set; }
    public float Top { get; set; }
    public float Bottom { get; set; }
    internal GameObject GameObject
    {
        get
        {
            return gameObject;
        }
        set
        {
            this.gameObject = value;
        }
    }

    public ClassView(GameObject gameObject, ulong id)
    {
        this.gameObject = gameObject;
        this.Id = id;
    }

    public void SetClassName(string className)
    {
        var background = gameObject.transform.Find("Background");
        var header = background.Find("Header");
        header.GetComponent<TextMeshProUGUI>().text = className;
    }
    public void SetTMProAttributes(List<AttributeModel> attributes)
    {
        if (gameObject)
        {
            var transformBackground = gameObject.transform.Find("Background");
            var transformAttributes = transformBackground.Find("Attributes");

            if (attributes != null)
            {
                string textualAttributes = "";
                foreach (AttributeModel attribute in attributes)
                {
                    textualAttributes += attribute.Name + ": " + attribute.Type + "\n";
                }
                transformAttributes.GetComponent<TextMeshProUGUI>().text = textualAttributes;
            }
        }
    }
    public void SetTMProMethods(List<Method> methods)
    {
        if (methods != null)
        {
            var transformBackground = gameObject.transform.Find("Background");
            var transformMethods = transformBackground.Find("Methods");

            foreach (Method method in methods)
            {
                string arguments = "(";
                if (method.arguments != null)
                {
                    for (int argumentIndex = 0; argumentIndex < method.arguments.Count; argumentIndex++)
                    {
                        if (argumentIndex < method.arguments.Count - 1)
                            arguments += (method.arguments[argumentIndex] + ", ");
                        else
                            arguments += (method.arguments[argumentIndex]);
                    }
                }
                arguments += ")";
                transformMethods.GetComponent<TextMeshProUGUI>().text += method.Name + arguments + " :" + method.ReturnValue + "\n";
            }
        }
    }
}
