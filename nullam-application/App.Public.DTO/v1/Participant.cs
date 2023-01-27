using Base.Domain;

namespace App.Public.DTO.v1;

public class Participant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PaymentTypeId { get; set; }
    public PaymentType? PaymentType { get; set; }

    public Guid? PersonId { get; set; }
    public Person? Person { get; set; }
    
    public Guid? EnterpriseId { get; set; }
    public Enterprise? Enterprise { get; set; }

    public Guid EventId { get; set; }
}