using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public string EventTime { get; set; } = default!;
    
    [MaxLength(256)]
    public string Location { get; set; } = default!;
    
    [MaxLength(1000, ErrorMessage = "Lisainfo on liiga suur, peab olema maksimaalselt 1000 tähemärki")]
    public string AdditionalDetails { get; set; } = default!;

    public bool IsHeld { get; set; } = false;
    
    public ICollection<Participant>? Participants { get; set; }
}