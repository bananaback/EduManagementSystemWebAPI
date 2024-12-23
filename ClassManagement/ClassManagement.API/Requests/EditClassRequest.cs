﻿using System.ComponentModel.DataAnnotations;

namespace ClassManagement.API.Requests
{
    public class EditClassRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
