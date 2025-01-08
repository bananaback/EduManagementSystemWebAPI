using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using ClassManagement.Application.Features.Students.Queries;
using MediatR;

namespace ClassManagement.Application.Features.Classes.Queries.GetById
{
    public class GetClassByIdCommandHandler : IRequestHandler<GetClassByIdCommand, ClassReadDto>
    {
        private readonly IClassRepository _classRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GetClassByIdCommandHandler(
            IClassRepository classRepository,
            IUnitOfWork unitOfWork)
        {
            _classRepository = classRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClassReadDto> Handle(GetClassByIdCommand command, CancellationToken cancellationToken)
        {
            var @class = await _classRepository.GetByIdAsync(command.Id, cancellationToken);

            if (@class == null)
            {
                throw new ClassRetrievalException($"Class with id {command.Id} not found.");
            }

            return new ClassReadDto
            {
                Id = command.Id,
                Name = @class.Name.Value,
                Description = @class.Description.Value,
                StartDate = @class.StartDate,
                EndDate = @class.EndDate,
                ClassStatus = @class.Status,
                MaxCapacity = @class.MaxCapacity,
                EnrolledStudents = @class.Enrollments.Select(
                    e => new EnrollmentReadDto
                    {
                        StudentId = e.Student.Id,
                        Name = e.Student.Name.FullName,
                        Email = (e.Student.ExposePrivateInfo) ? e.Student.Email.Value : "secret",
                        DateEnrolled = e.Student.EnrollmentDate,
                        DateEnrolledClass = e.EnrollmentDate
                    }
                ).ToList(),
            };
        }
    }
}
