using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Domain.Entities;
using ClassManagement.Domain.Enums;
using ClassManagement.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClassManagement.Application.BackgroundServices
{
    public class InboxReaderService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<InboxReaderService> _logger;

        public InboxReaderService(IServiceScopeFactory serviceScopeFactory, ILogger<InboxReaderService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var inboxRepository = scope.ServiceProvider.GetRequiredService<IInboxMessageRepository>();
                    var studentRepository = scope.ServiceProvider.GetRequiredService<IStudentRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var message = await inboxRepository.GetOldestUnprocessedAsync(stoppingToken);

                    if (message != null)
                    {
                        _logger.LogInformation($"Processing InboxMessage {message.Id} of type {message.Type}");

                        // Process the message based on its type
                        switch (message.Type)
                        {
                            case MessageType.STUDENTCREATED:
                                await HandleStudentCreated(message, studentRepository, stoppingToken);
                                break;

                            case MessageType.STUDENTUPDATED:
                                await HandleStudentUpdated(message, studentRepository, stoppingToken);
                                break;

                            case MessageType.STUDENTDELETED:
                                await HandleStudentDeleted(message, studentRepository, stoppingToken);
                                break;

                            default:
                                _logger.LogWarning($"Unknown message type: {message.Type}");
                                break;
                        }

                        // Mark the message as processed
                        message.MarkAsProcessed();
                        inboxRepository.Update(message);

                        // Commit changes to the database
                        await unitOfWork.SaveChangesAsync(stoppingToken);
                    }

                    _logger.LogInformation("InboxReaderService is working.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in InboxReaderService.");
                }

                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }

        private async Task HandleStudentCreated(InboxMessage message, IStudentRepository studentRepository, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Payload for STUDENTCREATED: {message.Payload}");

            try
            {
                using var document = System.Text.Json.JsonDocument.Parse(message.Payload);
                var root = document.RootElement;

                // Accessing nested fields
                var firstName = root.GetProperty("Name").GetProperty("FirstName").GetString();
                var lastName = root.GetProperty("Name").GetProperty("LastName").GetString();

                var emailString = root.GetProperty("Email").GetProperty("Value").GetString();

                var genderInt = root.GetProperty("Gender").GetInt32();
                var gender = (GenderEnum)genderInt; // Assuming GenderEnum maps to the integer values

                var dateOfBirth = DateOnly.FromDateTime(root.GetProperty("DateOfBirth").GetDateTime());
                var enrollmentDate = DateOnly.FromDateTime(root.GetProperty("EnrollmentDate").GetDateTime());

                var houseNumber = root.GetProperty("Address").GetProperty("HouseNumber").GetString();
                var street = root.GetProperty("Address").GetProperty("Street").GetString();
                var ward = root.GetProperty("Address").GetProperty("Ward").GetString();
                var district = root.GetProperty("Address").GetProperty("District").GetString();
                var city = root.GetProperty("Address").GetProperty("City").GetString();

                var address = new Address(houseNumber!, street!, ward!, district!, city!);

                var exposePrivateInfo = root.GetProperty("ExposePrivateInfo").GetBoolean();

                var id = root.GetProperty("Id").GetGuid();

                // Create domain objects
                var name = new PersonName(firstName!, lastName!);
                var email = new Email(emailString!);

                // Create Student object
                var student = new Student(
                    name,
                    email,
                    gender,
                    dateOfBirth,
                    enrollmentDate,
                    address,
                    exposePrivateInfo
                );

                student.UpdateId(id);

                // Save to the repository
                await studentRepository.AddAsync(student, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to parse payload: {message.Payload}");
                throw;
            }
        }



        private async Task HandleStudentUpdated(InboxMessage message, IStudentRepository studentRepository, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Payload for STUDENTUPDATED: {message.Payload}");

            try
            {
                using var document = System.Text.Json.JsonDocument.Parse(message.Payload);
                var root = document.RootElement;

                // Extract the student ID to retrieve the existing student
                var id = root.GetProperty("Id").GetGuid();

                // Fetch the existing student
                var existingStudent = await studentRepository.GetByIdAsync(id, cancellationToken);

                if (existingStudent == null)
                {
                    throw new InvalidOperationException($"Student with ID {id} not found.");
                }

                // Extract optional fields from the payload
                var name = root.TryGetProperty("Name", out var nameElement)
                    ? new PersonName(
                        nameElement.GetProperty("FirstName").GetString()!,
                        nameElement.GetProperty("LastName").GetString()!
                      )
                    : null;

                var email = root.TryGetProperty("Email", out var emailElement)
                    ? new Email(emailElement.GetProperty("Value").GetString()!)
                    : null;

                var gender = root.TryGetProperty("Gender", out var genderElement)
                    ? (GenderEnum?)genderElement.GetInt32()
                    : null;

                var dateOfBirth = root.TryGetProperty("DateOfBirth", out var dobElement)
                    ? (DateOnly?)DateOnly.FromDateTime(dobElement.GetDateTime())
                    : null;

                var enrollmentDate = root.TryGetProperty("EnrollmentDate", out var enrollmentElement)
                    ? (DateOnly?)DateOnly.FromDateTime(enrollmentElement.GetDateTime())
                    : null;

                var address = root.TryGetProperty("Address", out var addressElement)
                    ? new Address(
                        addressElement.GetProperty("HouseNumber").GetString()!,
                        addressElement.GetProperty("Street").GetString()!,
                        addressElement.GetProperty("Ward").GetString()!,
                        addressElement.GetProperty("District").GetString()!,
                        addressElement.GetProperty("City").GetString()!
                      )
                    : null;

                var exposePrivateInfo = root.TryGetProperty("ExposePrivateInfo", out var exposeElement)
                    ? (bool?)exposeElement.GetBoolean()
                    : null;

                // Update the student with the provided fields
                existingStudent.Update(
                    name,
                    email,
                    gender,
                    dateOfBirth,
                    enrollmentDate,
                    address,
                    exposePrivateInfo
                );

                // Persist changes
                studentRepository.Update(existingStudent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to process STUDENTUPDATED message: {message.Payload}");
                throw;
            }
        }


        private async Task HandleStudentDeleted(InboxMessage message, IStudentRepository studentRepository, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Payload for STUDENTDELETED: {message.Payload}");

            try
            {
                using var document = System.Text.Json.JsonDocument.Parse(message.Payload);
                var root = document.RootElement;

                // Extract the student ID from the payload
                var id = root.GetProperty("Id").GetGuid();

                // Fetch the existing student
                var existingStudent = await studentRepository.GetByIdAsync(id, cancellationToken);

                if (existingStudent != null)
                {
                    // Delete the student
                    studentRepository.Delete(existingStudent);
                    _logger.LogInformation($"Student with ID {id} has been successfully deleted.");
                }
                else
                {
                    _logger.LogWarning($"Student with ID {id} not found. No deletion occurred.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to process STUDENTDELETED message: {message.Payload}");
                throw;
            }
        }

    }
}
