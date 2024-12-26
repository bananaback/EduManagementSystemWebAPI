using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Classes.Commands.EnrollStudent
{
    public class EnrollStudentCommandHandler : IRequestHandler<EnrollStudentCommand, bool>
    {
        private readonly IClassRepository _classRepository;
        private readonly IStudentRepository _studentRepository; 
        private readonly IUnitOfWork _unitOfWork;

        public EnrollStudentCommandHandler(
            IClassRepository classRepository,
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork)
        {
            _classRepository = classRepository;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(EnrollStudentCommand command, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(command.StudentId, cancellationToken);

            if (student == null)
            {
                throw new StudentRetrievalException($"Student with id {command.StudentId} not found.");
            }

            var @class = await _classRepository.GetByIdAsync(command.ClassId, cancellationToken);

            if (@class == null)
            {
                throw new ClassRetrievalException($"Class with id {command.ClassId} not found.");
            }

            @class.EnrollStudent(student);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new StudentEnrollmentException($"Failed to save changes while trying to enroll student with id {command.StudentId} into class with id {command.ClassId}.");
            }

            return true;
        }
    }
}
