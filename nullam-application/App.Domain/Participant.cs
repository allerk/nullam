using Base.Domain;

namespace App.Domain;

public class Participant : DomainEntityMetaId
{
    public Guid PaymentTypeId { get; set; }
    public PaymentType? PaymentType { get; set; }

    public Guid? PersonId { get; set; }
    public Person? Person { get; set; }

    public Guid? EnterpriseId { get; set; }
    public Enterprise? Enterprise { get; set; }

    public Guid EventId { get; set; }
    public Event? Event { get; set; }
}