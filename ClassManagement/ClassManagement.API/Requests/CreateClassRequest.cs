using System.ComponentModel.DataAnnotations;

namespace ClassManagement.API.Requests
{
    public class CreateClassRequest
    {
        [Required]
        public string ClassName { get; set; } = string.Empty;
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public CreateClassRequest() { }
        public CreateClassRequest(string className, DateTime startDate, DateTime endDate)
        {
            ClassName = className;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
