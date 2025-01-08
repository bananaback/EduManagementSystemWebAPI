using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Enrollments.Commands.Update
{
    public class UpdateEnrollmentCommandHandler : IRequestHandler<UpdateEnrollmentCommand, bool>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateEnrollmentCommandHandler(IEnrollmentRepository enrollmentRepository, IUnitOfWork unitOfWork)
        {
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
        }   
        public async Task<bool> Handle(UpdateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentRepository.GetByClassIdAndStudentIdAsync(request.ClassId, request.StudentId, cancellationToken);

            if (enrollment == null)
            {
                throw new EnrollmentRetrievalException($"Enrollment with class id {request.ClassId} and student id {request.StudentId} not found.");
            }

            enrollment.Update(request.Grade, request.EnrollmentDate, request.Status);

            return true;
        }
    }
}
