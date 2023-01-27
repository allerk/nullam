using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Person : DomainEntityMetaId
{
    [MaxLength(128)]
    public string FirstName { get; set; } = default!;
    
    [MaxLength(128)]
    public string LastName { get; set; } = default!;
    
    [MaxLength(11)]
    public string IdentificationNumber { get; set; } = default!;
    
    [MaxLength(1500, ErrorMessage = "Lisainfo on liiga suur, peab olema maksimaalselt 1500 tähemärki")]
    public string AdditionalDetails { get; set; } = default!;

    public ICollection<Participant>? Participant { get; set; }
}