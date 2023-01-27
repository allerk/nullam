using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Enterprise : DomainEntityMetaId
{
    [MaxLength(128)]
    public string LegalName { get; set; } = default!;

    [MaxLength(20)]
    public string RegisterCode { get; set; } = default!;

    public int ParticipantsNumber { get; set; }

    [MaxLength(5000, ErrorMessage = "Lisainfo on liiga suur, peab olema maksimaalselt 5000 tähemärki")] 
    public string AdditionalDetails { get; set; } = default!;

    public ICollection<Participant>? Participant { get; set; }
}