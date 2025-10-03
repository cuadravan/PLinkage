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

## Architecture Overview of the Application (TBD; TODO)

* **MVVM Pattern** – Clean separation of UI, business logic, and data.
* **Service Layers** – Centralized logic for reusability and maintainability.
* **Repositories & Interfaces** – Abstracted data access for better testing and flexibility.
* **Dependency Injection** – Promotes modular, testable, and maintainable code.

---

## Architecture Overview of the Web API/ Backend

This project follows a **clean layered architecture** with inspiration from Domain-Driven Design (DDD) principles. The goal is to keep the codebase **modular, testable, and maintainable**, without over-engineering beyond the project scope.

### Layers & Responsibilities

* **Controllers**

  * Handle incoming HTTP requests and responses.
  * Delegate work to application services.
  * No business logic — they are purely endpoints.
  * Mapping to and from DTOS - they handle the mapping.

* **Application Services**

  * Contain **use case orchestration** (e.g., filtering skill providers, handling workflows).
  * Coordinate between repositories, specifications, and domain models.
  * Keep business processes consistent.

* **Entities**

  * Represent core domain objects (e.g., `SkillProvider`, `Project`, `Chat`).
  * Hold identity (`Id`, `UserId`) and core data.
  * Some business behavior may be added here in the future, but currently most rules are in services.

* **Value Objects**

  * Represent concepts defined by their values, not identity.
  * Example: `Location` (latitude/longitude) is immutable and compared by value.
  * Encapsulate small, strongly-typed domain concepts.

* **Specifications**

  * Encapsulate query/filtering logic in reusable classes.
  * Example: `SkillProviderByStatusAndLocationSpecification`.
  * Keeps repository queries expressive and composable.

* **Repositories**

  * Abstract persistence concerns (MongoDB in this case).
  * Return domain entities rather than database models.
  * Follow contracts defined by repository interfaces.

* **Interfaces**

  * Define contracts (`IRepository`, `IService`, `ISpecification`, etc.).
  * Support **dependency injection** and testability.

---

### DDD Inspiration

This architecture borrows from DDD but keeps things pragmatic:

* Entities and Value Objects represent the domain model.
* Application Services act as use case coordinators.
* Specifications capture reusable query logic.
* Repositories abstract persistence.

Unlike strict DDD, **most business logic currently lives in services rather than inside entities or aggregates**. This keeps the design simpler while still maintaining a strong separation of concerns.

---

### Benefits

* **Clean separation of concerns** → easier to maintain and scale.
* **Testability** → services and repositories can be unit tested in isolation.
* **Extensibility** → easy to add new services, specifications, or domain rules without breaking existing layers.
* **DDD-inspired structure** → clarity and modularity without over-complication.

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
