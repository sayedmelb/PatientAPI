**Patient API**

A comprehensive .NET 8 Web API for managing patients and prescriptions, built with Clean Architecture principles, MongoDB integration, and comprehensive validation.

**Table of Contents**

Overview

Architecture

Features

Technology Stack

Getting Started

Testing

**Overview**

The Patient API is a modern healthcare management system designed to handle patient records and prescription management. It follows Clean Architecture principles with clear separation of concerns, making it maintainable, testable, and scalable.

**Architecture**

The application follows **\*\*Clean Architecture\*\*** with four distinct layers:

- API Layer (Controllers)
- Application Layer (Services, DTOs, Interfaces)
- Domain Layer (Entities, Repositories)
- Infrastructure Layer (Data Access, External Services)

**Domain Layer**

- **Entities**: Core business objects (Patient, Prescription)
- **Value Objects**: Complex types (PatientWithPrescriptions)
- **Repositories:** Data access abstractions
- **Domain Services**: Business logic

**Application Layer**

- **Services**: Use case implementations
- **DTOs**: Data transfer objects
- **Mapping**: AutoMapper profiles
- **Interfaces:** Service contracts

**Infrastructure Layer**

- **Repositories**: MongoDB implementations
- **Configuration**: Database settings
- **Models**: MongoDB-specific models

**API Layer**

- **Controllers**: HTTP endpoints
- **Middleware**: Cross-cutting concerns
- **Filter**: Validation and error handling

**Features**

**Core Functionality**

- **Patient Management**: CRUD operations for patient records
- **Prescription Management**: CRUD operations for prescriptions
- **Search Capabilities**: Search patients by name and prescriptions by drug name
- **Patient-Prescription Relationships**: View patient with all prescriptions

**Technical Features**

- **Clean Architecture**: Maintainable and testable code structure
- **MongoDB Integration**: NoSQL database with flexible schema
- **Comprehensive Validation**: Request validation with detailed error messages
- **Error Handling**: Global exception handling with structured responses
- **AutoMapper**: Automatic object-to-object mapping
- **Logging**: Structured logging throughout the application
- **Result Pattern**: Consistent error handling pattern

**Technology Stack**

- **Framework**: .NET 8
- **Database**: MongoDB
- **ORM**: MongoDB.Driver
- **Mapping**: AutoMapper
- **Validation**: Data Annotations
- **Logging**: Microsoft.Extensions.Logging
- **Documentation**: Swagger/OpenAPI

**Getting Started**

**Prerequisites**

- .NET 8 SDK
- MongoDB (local installation or MongoDB Atlas)
- Visual Studio 2022 or VS Code

**Installation**

1\. **Clone the repository**

&nbsp;  bash

&nbsp;  git clone &lt;repository-url&gt;

&nbsp;  cd patient-api

2\. **Restore dependencies**

&nbsp;bash

&nbsp;dotnet restore

3\. **Configure MongoDB connection**

&nbsp;  Update \`appsettings.json\`:

&nbsp;  json

&nbsp;  {

&nbsp;    "DatabaseSettings": {

&nbsp;      "ConnectionString": "mongodb://localhost:27017",

&nbsp;      "DatabaseName": "PatientDB",

&nbsp;      "PatientsCollectionName": "patients",

&nbsp;      "PrescriptionsCollectionName": "prescriptions"

&nbsp;    }

&nbsp;  }

4\. **Run the application**

bash

&nbsp;dotnet run

**Testing**

**Unit Testing**

bash

dotnet test
