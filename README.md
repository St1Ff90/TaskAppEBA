# Task Management API

This project is a task management API built with .NET 8.0, following a clean architecture design pattern. It includes various components such as the Business Logic Layer (BL), Data Access Layer (DAL), Core, API, and Tests.

## Table of Contents

- [Setup Instructions](#setup-instructions)
- [API Documentation](#api-documentation)
- [Architecture and Design Choices](#architecture-and-design-choices)

## Setup Instructions

To run the project locally, follow these steps:

### Prerequisites

1. Docker: Ensure you have Docker installed on your machine.
2. Docker Compose: Docker Desktop includes Docker Compose; ensure it’s enabled.

### Clone the Repository

```bash
git clone https://github.com/St1Ff90/TaskAppEBA
cd TaskAppEBA
```

### Configure Database

#### Database Configuration: 

The project uses SQL Server. The connection string is specified in the docker-compose.yml file and is set to use the SA account. You can change the password and database name as needed.

Ensure that the connection string in the taskappeba service of your docker-compose.yml file is correct:

```yaml
  environment:
  - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=TaskDb;User Id=sa;Password=*********;Encrypt=False;Trust Server Certificate=True;  
```

### Build and Run the Project

    Open the project in your preferred IDE (e.g., Visual Studio, VS Code).

    Build the solution to restore dependencies.

    Run the API project:
```bash
    dotnet run --project API
```

### The API should now be running at https://localhost:5001 or http://localhost:5000.
## API Documentation

### The API exposes the following endpoints:
#### User Endpoints
##### Register a New User

    Endpoint: POST /users/register

    Request Body:

    json

    {
      "username": "string",
      "email": "string",
      "password": "string"
    }

    Response: 200 OK on success.

##### User Login

    Endpoint: POST /users/login

    Request Body:

    json

    {
      "email": "string",
      "password": "string"
    }

    Response:
        200 OK with JWT token on success.
        401 Unauthorized if credentials are invalid.

#### Task Endpoints
##### Retrieve User Tasks

    Endpoint: GET /tasks
    Query Parameters: Use filters such as status, priority, pageNumber, pageSize.
    Response:
        200 OK with the list of tasks.
        204 No Content if no tasks found.

##### Get Task by ID

    Endpoint: GET /tasks/{id}
    Response:
        200 OK with the task object if found.
        404 Not Found if the task does not exist.

##### Create a New Task

    Endpoint: POST /tasks

    Request Body:

    json

    {
      "title": "string",
      "description": "string",
      "dueDate": "yyyy-mm-dd",
      "priority": 1,
      "status": 1
    }

    Response:
        201 Created with the created task.
        500 Internal Server Error if an error occurs.

##### Update a Task

    Endpoint: PUT /tasks/{id}
    Request Body: Similar to create task body.
    Response:
        204 No Content if successfully updated.
        404 Not Found if the task does not exist.

##### Delete a Task

    Endpoint: DELETE /tasks/{id}
    Response:
        204 No Content if successfully deleted.
        404 Not Found if the task does not exist.

## Architecture and Design Choices
### Overview

#### The application is designed using a clean architecture approach, separating concerns into different layers:

    API: This layer handles HTTP requests, routing, and responses. It uses controllers to manage incoming requests and coordinate with the Business Logic Layer.

    Business Logic Layer (BL): Contains the core business logic and validation. This layer interacts with the Data Access Layer to perform operations and return data to the API layer.

    Data Access Layer (DAL): Responsible for data persistence and retrieval. It uses Entity Framework Core to manage the database context and perform CRUD operations.

    Core: Contains common models, DTOs, and utilities shared across the application.

#### Design Choices

    Entity Framework Core: Used for ORM to simplify database usage and manage migrations.

    JWT Authentication: Implemented for secure access to the API, allowing only authenticated users to access protected endpoints.

    Logging: Integrated with ILogger to provide into on the application acting and error handling.

    Dependency Injection: Used throughout the application for better testability and separation of concerns.

    DTOs (Data Transfer Objects): Used to define the data shape sent to and from the API, ensuring that the internal model is not exposed directly.

    Asynchronous Programming: Employed throughout the application to enhance performance and responsiveness.

## Conclusion

This project serves as a robust foundation for a task management application, demonstrating best practices in software architecture, design patterns, and modern .NET development. For any questions or issues, feel free to open an issue in the repository.
