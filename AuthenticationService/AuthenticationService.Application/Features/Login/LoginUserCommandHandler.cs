using AuthenticationService.Application.Common.Interfaces;
using AuthenticationService.Application.Common.Interfaces.Authenticators;
using AuthenticationService.Application.Common.Interfaces.PashwordHashers;
using AuthenticationService.Application.Common.Interfaces.Repository;
using AuthenticationService.Application.Exceptions;
using AuthenticationService.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthenticatedUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthenticator _authenticator;
        
        public LoginUserCommandHandler(IUserRepository userRepository, 
            IUnitOfWork unitOfWork, 
            IPasswordHasher passwordHasher,
            IAuthenticator authenticator)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _authenticator = authenticator;
        }

        public async Task<AuthenticatedUserResult> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            Username username = Username.Create(command.UserName);
            Password password = Password.Create(command.Password);

            var existingUser = await _userRepository.GetByUsernameAsync(command.UserName, cancellationToken);

            if (existingUser == null)
            {
                throw new UserRetrievalException($"User with username {command.UserName} not found.");
            }

            bool isCorrectPassword = existingUser.HashPassword.Value == _passwordHasher.HashPassword(password.Value);

            if (isCorrectPassword)
            {
                return await _authenticator.Authenticate(existingUser);
            }

            return AuthenticatedUserResult.Failure();
        }
    }
}
