using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClassView
{
    public ulong Id { get; set; }
    public float Left { get; set; }
    public float Right { get; set; }
    public float Top { get; set; }
    public float Bottom { get; set; }
    internal GameObject GameObject { get; set; }
    public ClassView(GameObject gameObject, ulong id)
    {
        this.GameObject = gameObject;
        this.Id = id;
    }
    public void SetClassProperty(string propertyName, string propertyValue)
    {
        GameObject.transform
            .Find("Background")
            .Find(propertyName)
            .GetComponent<TextMeshProUGUI>()
            .text = propertyValue;
    }
    public static string AttributesToString(List<AttributeModel> attributes)
    {
        if (attributes == null)
            return "";
        var textualAttributes = "";
        foreach (var attribute in attributes)
        {
            textualAttributes += attribute.Name + ": " + attribute.Type + "\n";
        }
        return textualAttributes;
    }
    public static string MethodsToString(List<Method> methods)
    {
        if (methods == null)
            return "";
        var result = "";
        foreach (var method in methods)
        {
            var arguments = "(";
            if (method.arguments != null)
            {
                for (var argumentIndex = 0; argumentIndex < method.arguments.Count; argumentIndex++)
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
        return result;
    }
}
