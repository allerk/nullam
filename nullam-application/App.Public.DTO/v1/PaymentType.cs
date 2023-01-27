using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class PaymentType
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(128)]
    public string Name { get; set; } = default!;

    [MaxLength(512)]
    public string Comment { get; set; } = default!;

    public ICollection<Participant>? Participants { get; set; }
}