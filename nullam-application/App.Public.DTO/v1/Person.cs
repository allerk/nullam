using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Person
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(128)]
    public string FirstName { get; set; } = default!;
    
    [MaxLength(128)]
    public string LastName { get; set; } = default!;
    
    [MaxLength(11)]
    public string IdentificationNumber { get; set; } = default!;
    
    [MaxLength(1500)]
    public string AdditionalDetails { get; set; } = default!;
}