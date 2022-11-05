using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ClassModel
{
    internal List<AttributeModel> Attributes { get; set; }
    internal List<Method> Methods { get; set; }
    public string Name { get; set; }
    public ulong Id { get; set; }

    public ClassModel()
    {
    }

    public ClassModel(ulong id)
    {
        Id = id;
    }

    public ClassModel(string name)
    {
        Name = name;
    }
}
