﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Students.Commands.Delete
{
    public class DeleteStudentCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
    }
}
