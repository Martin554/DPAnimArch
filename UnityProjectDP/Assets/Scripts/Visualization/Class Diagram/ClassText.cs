using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ClassTextChangedEvent : UnityEvent<string[]> { };
public class ClassText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text headerText;

    [SerializeField]
    private TMP_Text attributesText;

    [SerializeField]
    private TMP_Text methodsText;

    [HideInInspector]
    public UnityEvent<string[]> onTextChanged = new ClassTextChangedEvent();

    public string GetHeaderText()
    {
        return headerText.text;
    }
    public string GetAttributesText()
    {
        return attributesText.text;
    }
    public string GetMethodsText()
    {
        return methodsText.text;
    }

    public void SetText(string header, string attributes, string methods)
    {
        List<string> changed = new List<string>();
        if (!headerText.text.Equals(header))
        {
            changed.Add("Header");
            changed.Add(header);
        }

        if (!attributesText.text.Equals(attributes))
        {
            changed.Add("Attributes");
            changed.Add(attributes);
        }

        if (!methodsText.text.Equals(methods))
        {
            changed.Add("Methods");
            changed.Add(methods);
        }

        if (changed.Count > 0)
        {
            onTextChanged.Invoke(changed.ToArray());
        }
        SetHeader(header);
        SetAttributes(attributes);
        SetMethods(methods);
    }

    public void SetHeader(string header)
    {
        headerText.text = header;
    }
    public void SetAttributes(string attributes)
    {
        attributesText.text = attributes;
    }
    public void SetMethods(string methods)
    {
        methodsText.text = methods;
    }
}
