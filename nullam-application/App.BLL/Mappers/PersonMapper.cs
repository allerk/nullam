﻿using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PersonMapper : BaseMapper<Person, App.DAL.DTO.Person>
{
    public PersonMapper(IMapper mapper) : base(mapper)
    {
    }
}