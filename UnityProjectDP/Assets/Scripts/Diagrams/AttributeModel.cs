public class AttributeModel
{
    public string Type { get; set; }
    public string Name { get; set; }
    public string Id { get; set; }
    public AttributeModel(string id, string name, string type)
    {
        this.Id = id;
        this.Type = type;
        this.Name = name;
    }
    public AttributeModel(string Name, string Type)
    {
        this.Name = Name;
        this.Type = Type;
    }
    public AttributeModel()
    {
    }
}
