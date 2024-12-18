# EduManagementSystemWebAPI  

A microservices-based educational management system for handling students and classes. Built with **Clean Architecture**, **CQRS**, this project ensures scalability, maintainability, and testability.  

## Features  

### Student Management Service  
- Add new students.  
- Edit existing student information.  
- Delete student records.  

### Class Management Service  
- Add new classes.  
- Edit existing class information.  
- Delete class.
- Enroll students in classes.
- View a list of students in a specific class.  

## Technology Stack  
- **.NET 8**  
- **MediatR** for CQRS implementation.  
- **RabbitMQ** for communication between services.  
- **Entity Framework Core** for database access.  
- **Docker** (for deployment).  

## Architecture  
The project follows the principles of **Clean Architecture**, separating concerns into multiple layers:  
1. **Domain**: Business rules and core logic (Entities, Value Objects, Domain Events).  
2. **Application**: Use cases (Commands and Queries with MediatR).  
3. **Infrastructure**: Data access and external integrations.  
4. **API**: Exposes endpoints for communication.  

Each microservice adheres to this architecture.  

## How to Run Locally  
To be writing...