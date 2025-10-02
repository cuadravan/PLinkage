# PLinkage

*Project Linkage Application*

**Tagline:** *Linking Busy Hands with Busy Minds*

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge\&logo=csharp\&logoColor=white)
![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-5C2D91?style=for-the-badge\&logo=.net\&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge\&logo=.net\&logoColor=white)
![Azure](https://img.shields.io/badge/azure-%230072C6.svg?style=for-the-badge\&logo=microsoftazure\&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-%234ea94b.svg?style=for-the-badge\&logo=mongodb\&logoColor=white)

**PLinkage** is a **cross-platform .NET MAUI application** (Android & Windows) that connects **project owners** with **skill providers**. It integrates with an **ASP.NET Core Web API** hosted on **Azure App Service** and uses **MongoDB Atlas** as the database.

The app simplifies project collaboration through **project management, user profiles, real-time messaging, location-based matching, and contract workflows**.

---

## Features

* **Project Posting & Management**  
  Create, update, and manage projects easily as a project owner.

* **User Profiles**  
  Showcase skills and experience for skill providers.

* **Messaging System**  
  Chat feature to facilitate communication between users.

* **Location-Based Matching**  
  Suggests projects or skill providers nearby for easier collaboration.

* **Contract Workflow**  
  Streamlined process for negotiation, agreement, and tracking of project contracts.

* **Dashboard & Quick Stats**  
  Monitor ongoing linkages, active projects, and received proposals at a glance.

---

## Architecture & Design

* **MVVM Pattern** – Clean separation of UI, business logic, and data.
* **Service Layers** – Centralized logic for reusability and maintainability.
* **Repositories & Interfaces** – Abstracted data access for better testing and flexibility.
* **Dependency Injection** – Promotes modular, testable, and maintainable code.

---

## Getting Started

Follow these steps to run the app locally:

1. **Clone the repository:**

   ```bash
   git clone https://github.com/cuadravan/PLinkage.git
   ```

2. **Open the solution in Visual Studio 2022** (ensure the .NET MAUI workload is installed).

3. **Restore NuGet packages:**

   ```bash
   dotnet restore
   ```

4. **Build and run the application:**

   ```bash
   dotnet build
   dotnet run
   ```

---

## Tech Stack

* **Frontend:** .NET MAUI (Android & Windows)
* **Backend:** ASP.NET Core Web API
* **Database:** MongoDB Atlas
* **Hosting:** Azure App Service

---
