namespace cams.application.models;

public class Bidder
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public Bidder(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    
}