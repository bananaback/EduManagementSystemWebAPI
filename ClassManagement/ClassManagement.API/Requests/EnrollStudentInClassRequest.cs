using System.ComponentModel.DataAnnotations;

namespace ClassManagement.API.Requests
{
    public class EnrollStudentInClassRequest
    {
        [Required]
        public Guid StudentId { get; set; }
    }
}
