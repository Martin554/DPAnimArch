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
        gameObject.transform
            .Find("Background")
            .Find("Header")
            .GetComponent<TextMeshProUGUI>()
            .text = className;
    }
    public string attributesToString(List<AttributeModel> attributes)
    {
        string result = "";
        if (attributes != null)
        {
            string textualAttributes = "";
            foreach (AttributeModel attribute in attributes)
            {
                textualAttributes += attribute.Name + ": " + attribute.Type + "\n";
            }
            result = textualAttributes;
        }
        return result;
    }
    public void SetTMProAttributes(List<AttributeModel> attributes)
    {
        if (gameObject)
        {
            gameObject.transform
                .Find("Background")
                .Find("Attributes")
                .GetComponent<TextMeshProUGUI>()
                .text = attributesToString(attributes);
        }
    }

    public string methodsToString(List<Method> methods)
    {
        string result = "";
        if (methods != null)
        {
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
                result += method.Name + arguments + " :" + method.ReturnValue + "\n";
            }
        }
        return result;
    }
    public void SetTMProMethods(List<Method> methods)
    {
        if (methods != null)
        {
            gameObject.transform
                .Find("Background")
                .Find("Methods")
                .GetComponent<TextMeshProUGUI>()
                .text = methodsToString(methods);
        }
    }
}
