using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class Class
{
    private List<Attribute> attributes;
    private List<Method> methods;
    private GameObject gameObject;
    public string Name { get; set; }
    public string XmiId { get; set; }
    public string Visibility { get; set; }
    public string NameSpc { get;  set; }
    public string Geometry { get; set; }
    public float Left { get; set; }
    public float Right { get; set; }
    public float Top { get; set; }
    public float Bottom { get; set; }
    public string Type { get; set; }
    internal List<Attribute> Attributes { get; set; }
    internal List<Method> Methods { get; set; }
    internal GameObject GameObject
    {
        get
        {
            return gameObject;
        }
        set
        {
            this.gameObject = value;
            this.gameObject.name = this.Name;
            var background = this.gameObject.transform.Find("Background");
            var header = background.Find("Header");
            header.GetComponent<TextMeshProUGUI>().text = this.Name;
        }
    }

    public void SetTMProAttributes()
    {
        if (gameObject)
        {
            var transformBackground = gameObject.transform.Find("Background");
            var transformAttributes = transformBackground.Find("Attributes");

            if (Attributes != null)
            {
                string textualAttributes = "";
                foreach (Attribute attribute in Attributes)
                {
                    textualAttributes += attribute.Name + ": " + attribute.Type + "\n";
                }
                transformAttributes.GetComponent<TextMeshProUGUI>().text = textualAttributes;
            }
        }
    }
    public void SetTMProMethods()
    {
        if (Methods != null)
        {
            var background = gameObject.transform.Find("Background");
            var methods = background.Find("Methods");

            foreach (Method method in Methods)
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
                methods.GetComponent<TextMeshProUGUI>().text += method.Name + arguments + " :" + method.ReturnValue + "\n";
            }
        }
    }
    public Class()
    {
    }

    public Class(string name)
    {
        Name = name;
    }

    public Class(string name, List<Attribute> attributes, List<Method> methods)
    {
        this.Name = name;
        this.Attributes = attributes;
        this.Methods = methods;
        Left = 0f;
        Top = 0f;
    }
    public Class(string name, List<Attribute> attributes, List<Method> methods, float left, float top)
    {
        this.Name = name;
        this.Attributes = attributes;
        this.Methods = methods;
        this.Left = left;
        this.Top = top;
    }
}
