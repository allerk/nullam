using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Enterprise
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(128)]
    public string LegalName { get; set; } = default!;

    [MaxLength(20)]
    public string RegisterCode { get; set; } = default!;

    public int ParticipantsNumber { get; set; }

    [MaxLength(5000)] 
    public string AdditionalDetails { get; set; } = default!;
}