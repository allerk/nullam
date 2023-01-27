﻿using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO;

public class Event : DomainEntityId
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public DateTime EventTime { get; set; }
    
    [MaxLength(256)]
    public string Location { get; set; } = default!;

    [MaxLength(1000)]
    public string AdditionalDetails { get; set; } = default!;

    public ICollection<Participant>? Participants { get; set; }
}