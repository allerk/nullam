﻿using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class PersonMapper : BaseMapper<Person, App.Domain.Person>
{
    public PersonMapper(IMapper mapper) : base(mapper)
    {
    }
}