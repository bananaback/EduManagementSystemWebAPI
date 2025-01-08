﻿using ClassManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClassManagement.API.Requests
{
    public class EditClassRequest
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
        public ClassStatus? Status { get; set; }
        public byte? MaxCapacity { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object>? AdditionalData { get; set; }
    }
}
