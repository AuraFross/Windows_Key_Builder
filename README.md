# Windows Key Builder

A web application for generating customized Windows `autounattend.xml` files. This tool helps you create automated Windows installation media with ease.

**[🌐 Live Demo: https://wkey.aunixservers.com/](https://wkey.aunixservers.com/)**

## Preview

![Application Screenshot](assets/Images%20FUll%20Page.PNG)

## Features

- **Web Interface**: Modern UI built with Blazor and MudBlazor.
- **Docker Ready**: Fully containerized for easy deployment.
- **Comprehensive Support**: Includes all Windows 11 languages, time zones, and keyboard layouts.
- **Advanced Tweaks**:
  - Bypass TPM 2.0 & RAM checks for older hardware.
  - Automate disk partitioning.
  - Create user accounts automatically.
  - Debloat Windows by removing pre-installed apps.

## Getting Started

### Prerequisites

- Docker Desktop or Docker Engine

### Deployment

1. Run the container using Docker Compose:
   ```bash
   docker-compose up -d
   ```

2. Access the application at `http://localhost:8082` or `http://<server-ip>:8082`.

## Tech Stack

- **Framework**: .NET 6.0 (ASP.NET Core / Blazor Server)
- **UI Library**: MudBlazor
- **Containerization**: Docker
