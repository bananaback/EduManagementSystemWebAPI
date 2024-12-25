using AuthenticationService.Application.Common.Interfaces;
using AuthenticationService.Application.Common.Interfaces.PashwordHashers;
using AuthenticationService.Application.Common.Interfaces.Repository;
using AuthenticationService.Application.Exceptions;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RegisterUserCommandHandler(
            IPasswordHasher passwordHasher,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var existingUserWithUsername = await _userRepository.GetByUsernameAsync(command.Username, cancellationToken);

            if (existingUserWithUsername != null)
            {
                throw new DuplicatedUserException($"User with user name {command.Username} already exist.");
            }

            Username username = Username.Create(command.Username);

            Password password = Password.Create(command.Password);

            var newUser = new ApplicationUser
            (
                username,
                PasswordHash.Create(_passwordHasher.HashPassword(password.Value))
            );

            await _userRepository.AddAsync(newUser, cancellationToken);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new UserPersistenceException("Failed to save changes when creating user.");
            }

            return newUser.Id;
        }
    }
}
