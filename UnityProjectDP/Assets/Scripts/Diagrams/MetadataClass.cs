using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MetadataClass
{
    internal List<AttributeModel> Attributes { get; set; }
    internal List<Method> Methods { get; set; }
    public string Name { get; set; }
    public ulong Id { get; set; }

    public MetadataClass()
    {
    }

    public MetadataClass(ulong id)
    {
        Id = id;
    }

    public MetadataClass(string name)
    {
        Name = name;
    }
}
