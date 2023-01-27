using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO;

public class Enterprise : DomainEntityId
{
    [MaxLength(128)]
    public string LegalName { get; set; } = default!;

    [MaxLength(20)]
    public string RegisterCode { get; set; } = default!;

    public int ParticipantsNumber { get; set; }

    [MaxLength(5000)] 
    public string AdditionalDetails { get; set; } = default!;

    public ICollection<Participant>? Participant { get; set; }
}