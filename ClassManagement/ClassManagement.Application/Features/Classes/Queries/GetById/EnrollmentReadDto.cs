﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Classes.Queries.GetById
{
    public class EnrollmentReadDto
    {
        public Guid StudentId {  get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateEnrolled {  get; set; }
        public DateTime DateEnrolledClass { get; set; }
    }
}
