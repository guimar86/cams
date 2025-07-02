namespace cams.contracts.models;

/// <summary>
/// Represents a bidder with a unique identifier and a name.
/// </summary>
public class Bidder
{
    /// <summary>
    /// Gets or sets the unique identifier of the bidder.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the bidder.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bidder"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the bidder.</param>
    /// <param name="name">The name of the bidder.</param>
    public Bidder(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    
}