using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class Class
{
    private List<AttributeModel> attributes;
    private List<Method> methods;
    private GameObject gameObject;
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string XmiId { get; set; }
    public string Geometry { get; set; }
    public float Left { get; set; }
    public float Right { get; set; }
    public float Top { get; set; }
    public float Bottom { get; set; }
    public string Type { get; set; }
    internal List<AttributeModel> Attributes { get; set; }
    internal List<Method> Methods { get; set; }

    public Class()
    {
    }
}
