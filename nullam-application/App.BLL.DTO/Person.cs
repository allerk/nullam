using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO;

public class Person : DomainEntityId
{
    [MaxLength(128)]
    public string FirstName { get; set; } = default!;
    
    [MaxLength(128)]
    public string LastName { get; set; } = default!;
    
    [MaxLength(11)]
    public string IdentificationNumber { get; set; } = default!;
    
    [MaxLength(1500)]
    public string AdditionalDetails { get; set; } = default!;

    public ICollection<Participant>? Participant { get; set; }
}