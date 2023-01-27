using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class PaymentType : DomainEntityId
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;

    [MaxLength(512)]
    public string Comment { get; set; } = default!;

    public ICollection<Participant>? Participants { get; set; }
}