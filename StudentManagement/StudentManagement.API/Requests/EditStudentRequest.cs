namespace StudentManagement.API.Requests
{
    public class EditStudentRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
    }
}
