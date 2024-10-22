
# System File Management API

## Table of Contents
- [System File Management API](#system-file-management-api)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Project Structure](#project-structure)
    - [Domain Layer](#domain-layer)
    - [Application Layer](#application-layer)
    - [Infrastructure Layer](#infrastructure-layer)
    - [Features](#features)
    - [API](#api)
    - [Docker](#docker)
  - [Technologies Used](#technologies-used)
  - [How to Run](#how-to-run)
  - [License](#license)

## Overview

The System File Management API is designed to handle file storage and management similar to MEGA. The architecture is based on Onion or Clean Architecture with multiple layers for domain, application logic, infrastructure, and external services. This project uses MinIO for file storage and PostgreSQL as the database.

## Project Structure

- **API**: The main entry point for the application.
- **Core**: Contains the following libraries:
  - **Application**: Handles business logic, validations, and exceptions.
  - **Domain**: Holds entities, enums, and common logic (e.g., auditing).
- **Infrastructure**: Contains:
  - **Persistence**: Handles database access, configurations, and repository implementation.
  - **External**: Manages external services like MinIO and email services.

### Domain Layer

- **Entities**: 
  - `User`: Represents users with collections of `FilePacks`.
  - `FilePack`: Contains data such as name, description, and documents.
  - `Document`: Stores details like document type, description, content type, and path.
- **Enums**:
  - `DocumentType`: Defines the type of document.
  - `UserRole`: Defines roles like Admin, Personal, and Enterprise.
  
### Application Layer

- **Common**: Contains:
  - Validation behavior using FluentValidation.
  - Various exceptions for error handling.
- **Features**: Includes commands and queries for:
  - `Document` (Create, Delete, Update, View).
  - `FilePack` (Create, Delete, Update, View).
  - `User` (Login, Register, Confirm Email, etc.).
- **Models**: DTOs for pagination, filtering, and returning documents.

### Infrastructure Layer

- **Persistence**:
  - `DbContext`: `SystemFileDbContext` handles database interactions.
  - Repository implementation using a generic repository pattern.
  - Service registration using extensions for automatic repository implementations.
- **External**:
  - Services for file management using MinIO.
  - Email services for user notifications.
  - Helpers for file extensions, password hashing, OTP, and more.
  
### Features

- **Commands**:
  - `Document`: Create, Delete, Update.
  - `FilePack`: Create, Delete, Update.
  - `User`: Confirm Email, Login, Register, etc.
- **Queries**:
  - `Document`: Get, GetAll, GetMultiple (e.g., by type or choise as a zip file).
  - `FilePack`: Similar structure to documents.
  - `User`: Query user details.
  
### API

- **Middleware**: Handles exception logging.
- **Program.cs**: Configures:
  - Authentication (JWT bearer).
  - Swagger with JWT support.
  - Service registrations for settings, etc.
- **Controllers**: 
  - `DocumentController`
  - `FilePackController`
  - `UserController`

### Docker

- `Dockerfile`: Builds the API service.
- `docker-compose.yml`: Sets up the application using placeholders for MinIO and PostgreSQL. To run the project, clone the repository and use Docker Compose to launch it.

## Technologies Used

- **ASP.NET Core** (.NET 8)
- **MinIO** for file storage
- **PostgreSQL** for the database
- **FluentValidation** for request validation
- **JWT** for authentication
- **Swagger** for API documentation

## How to Run

1. Clone the repository.
2. Make sure Docker and Docker Compose are installed.
3. Run the application:
   ```bash
   docker-compose up
   ```

## License

This project is licensed under the MIT License.
