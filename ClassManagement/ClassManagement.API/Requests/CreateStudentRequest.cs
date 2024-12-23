namespace ClassManagement.API.Requests
{
    public class CreateStudentRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
    }
}
