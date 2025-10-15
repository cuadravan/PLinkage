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

## 🏗️ Architecture & Engineering of the Backend System (Web API)

This backend system is built on a **Clean, Layered Architecture**, reflecting a commitment to creating software that is **maintainable, scalable, and resilient**. The design is intentionally modular, adhering to foundational software engineering principles while making pragmatic trade-offs suitable for the project's scope.

### Guiding Principles in Practice

*   **Separation of Concerns (SoC):** The codebase is structured into distinct layers, each with a single, well-defined responsibility. This isolation makes the system easier to reason about, modify, and debug.
*   **Dependency Inversion Principle (DIP):** High-level modules (like Application Services) do not depend on low-level modules (like Repositories). Both depend on abstractions (interfaces). This is enforced through a rich set of interfaces (`IRepository`, `IService`), enabling...
*   **Testability:** By relying on abstractions, we can easily inject mock dependencies in unit tests. This allows for comprehensive testing of business logic in isolation from infrastructure concerns like databases.

### Architectural Layers & Design Rationale

| Layer | Responsibility | Key Principle Demonstrated |
| :--- | :--- | :--- |
| **🗣️ Controllers** | Thin adapters for the web. Handle HTTP request/response cycles, deserialization, and routing. | **Interface Segregation:** Controllers are focused solely on being HTTP entry points. |
| **⚙️ Application Services (The Orchestrators)** | Contain all use case orchestration and business logic. They coordinate anemic domain models to fulfill complex workflows. | **Single Responsibility Principle:** Services are scoped to specific business operations, keeping workflow logic consolidated and explicit. |
| **🏛️ Domain Layer (Anemic Models)** | `SkillProvider`, `Project`, and `Chat` are simple data holders (Entities defined by identity). `Location` is an immutable Value Object. | **Pragmatism over Dogma:** The anemic model is a conscious choice for rapid iteration and clarity in early-stage development. |
| **💾 Repositories** | Abstract data persistence (MongoDB). They return domain entities, hiding the complexities of the data layer. | **Persistence Ignorance:** The core logic is completely decoupled from the database, simplifying testing. |

### Key Design Decisions & Trade-offs

#### 1. The Pragmatic Choice: Anemic Domain Model with Orchestrating Services

*   **Decision:** All business logic and use case orchestration resides in **Application Services**, while Entities and Value Objects act as simple data containers.
*   **Rationale & Trade-off:** This approach was chosen for its **simplicity and clarity** in the early stages of the project. It avoids the complexity of a rich domain model, which can be premature optimization for a codebase whose core domain logic is still evolving.
*   **Future-Proofing:** The clean architecture provides a clear path for refactoring. As the domain matures and certain invariants or business rules become stable, they can be migrated from services into the relevant Entities and Value Objects, progressively enriching the domain model without a major overhaul. This demonstrates a **pragmatic, evolutionary approach to software design.**

#### 2. Explicit Contracts over Implicit Magic:

*   **Decision:** Heavy use of interfaces (`IRepository<T>`, `ISpecification<T>`) to define clear contracts between layers.
*   **Benefit:** This decouples layers, enabling true unit testing and making dependencies explicit. It also simplifies future changes (e.g., swapping a MongoDB repository for a SQL one).

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
