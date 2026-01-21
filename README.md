# Windows Key Builder

A Blazor Server web application for generating Windows `autounattend.xml` files. This project is a web-based adaptation of the UnattendBuilder tool, running in a containerized environment.

## Features

- **Web Interface**: Modern UI built with Blazor and MudBlazor.
- **Docker Ready**: Fully containerized with Docker support.
- **XML Generation**: Generates valid Windows Answer Files for automated installation.

## Getting Started

### Prerequisites

- Docker Desktop or Docker Engine
- .NET 6.0 SDK (for local development)

### Running with Docker

1. Build and run the container:
   ```bash
   docker-compose up -d --build
   ```

2. Access the application at `http://localhost:8082` (or the port defined in docker-compose.yml).

### Local Development

1. Restore dependencies:
   ```bash
   dotnet restore
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

## Tech Stack

- **Framework**: .NET 6.0 (ASP.NET Core / Blazor Server)
- **UI Library**: MudBlazor
- **Containerization**: Docker
