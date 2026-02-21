# PLinkage [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) 
[![Build and deploy .NET Core application to Web App PLinkageAPI](https://github.com/cuadravan/PLinkage/actions/workflows/PLinkageAPI.yml/badge.svg)](https://github.com/cuadravan/PLinkage/actions/workflows/PLinkageAPI.yml)

<img width="1920" height="1080" alt="PLinkage Cross-Platform Project Collaboration App" src="https://github.com/user-attachments/assets/dce8c0c7-d3c5-48b2-9523-f771350492ed" />

*Project Linkage/Collaboration Application*

**Tagline:** *Linking Busy Hands with Busy Minds*

**Context:** This is a school project. I have listed the reasons for the decisions made in the software engineering of this application.

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge\&logo=csharp\&logoColor=white)
![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-5C2D91?style=for-the-badge\&logo=.net\&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge\&logo=.net\&logoColor=white)
![Azure](https://img.shields.io/badge/azure-%230072C6.svg?style=for-the-badge\&logo=microsoftazure\&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-%234ea94b.svg?style=for-the-badge\&logo=mongodb\&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/github%20actions-%232671E5.svg?style=for-the-badge&logo=githubactions&logoColor=white)

**PLinkage** is a **cross-platform .NET MAUI application** (Android & Windows) that connects **project owners** with **skill providers**. It integrates with an **ASP.NET Core Web API** hosted on **Azure App Service** using **MongoDB Atlas** as the database. The deployment process is fully automated via a **GitHub Actions CI/CD pipeline**, which I configured to manage monorepo build triggers and secure cloud authentication.

The app simplifies project collaboration through **project management, user profiles, real-time messaging, location-based matching, and contract workflows**.

This project demonstrates ability in **cross-platform development, RESTful API design, and cloud database integration**. It showcases the practical application of industry-standard design patterns like MVVM and Clean Architecture.

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

## Client Application Architecture

The client uses the **MVVM (Model-View-ViewModel)** pattern to separate UI from business logic. This allows the app to run on both desktop and mobile platforms while sharing most of the codebase.

### Core Design Principles

**A. Cross-Platform Code Sharing:** ViewModels and majority of the Views are written once and shared across all platforms.

**B. Separation of Concerns:** The UI doesn't know about business logic, and business logic doesn't know about the UI.

**C. Reusable Components:** Common UI elements are built as reusable controls to avoid duplicating code and maintain consistent design.

### Architecture Layers

| Layer | What It Does |
|:------|:------------|
| **Views** | Handles what users see and interact with. Contains layouts with minimal code-behind. |
| **ViewModels** | Contains the app's state and logic (mostly validation only). |
| **Service Clients** | Handles all HTTP communication with the backend API. Takes care of serialization, error handling, and provides clean data to ViewModels. |
| **Services** | Provides infrastructure like navigation and dialog through interfaces. |

### Key Implementation Details

**1. Majority of ViewModels are Platform-Agnostic ViewModels**

ViewModels **don't know which platform they're running on**. They **only expose properties and commands** that Views bind to. While most ViewModels are platform-agnostic, specific scenarios (like the Chat module) utilize Platform-Specific Behavior. Rather than duplicating the entire ViewModel, I implemented platform checks to handle the distinct UI paradigms of Android vs. Windows, maximizing code reuse while respecting platform guidelines.

**2. Mixture of Platform-Agnostic and Platform-Specific Views**

As mentioned earlier, the Views used in this client application are divided into whether they are shared across all platforms and whether they aren't. This was done due to the **need for the client application to cater to the platform, more specifically, the platform's screen size**. For Android, screen size is limited and required Views that looked simpler. For Windows, screen size is vast and required Views to take advantage of this and have more complex layouts. Hence, **Views that rely on displaying a lot of data are mostly platform-specific**, to be precise, most of these are the root pages. For **Views that only rely on taking input**, they are platform-agnostic (albeit, a minor .NET MAUI syntax is added to adjust the padding of these views, but for the most part they are the 99% the same).

**3. Reusable Controls through ContentViews**

Common UI patterns (buttons, cards, list items) are extracted into ContentView components. Although setting up a "component-based UI" in .NET MAUI proved to be challenging due to the strict typing of the language, this decision was made due to the lengthy nature of .NET MAUI's UI development and the more numerous nature of the Views needed in this client application. The efficiency of **reusing the controls across all the Views** and the **ability to update numerous views** outweighed the difficult nature of the setup needed.

**4. Service Client Pattern**

The ViewModels in this client application are only responsible for validation and **calling/consuming the API is directed towards the injected ServiceClient**. This decision was made to **avoid duplicating the logic needed to call the API across all ViewModels**, and to also keep the ViewModels focused on their purpose. Additionally, the backend logic has been moved to a backend server to **maintain that the client application is only responsible for displaying and taking information from the user** (greatly enhancing security and separation).


**5. Dependency Injection**

This applications makes use of the Dependency Injection capabilities of .NET MAUI. This ensures that the functionalities are not strictly tied to their implementations, and that they could be swapped if needed. Most notably, the services used `Navigation Service` and `DialogService` are injected to ViewModels through DI so that the ViewModels are not tightly coupled with MAUI and can be tested with pure C#.

---

## Backend Architecture

The backend is a RESTful API built on **Clean Architecture** principles. It separates concerns into distinct layers, ensuring that the core domain logic remains independent of external concerns like the database or HTTP framework. The architecture adopts a **pragmatic approach to Domain-Driven Design (DDD)**, balancing strict encapsulation with development efficiency.

### Core Design Principles

**A. Hybrid Domain-Centric Design:**
The system utilizes **Rich Domain Models** where entities encapsulate their own internal business logic (e.g., state validation). However, complex workflows involving multiple entities are orchestrated by the Service layer to maintain separation of concerns.

**B. Separation of Concerns:**
* **Controllers** handle HTTP communication (request/response).
* **Services** contain the application business rules and orchestration.
* **Repositories** abstract the database access.
* **Entities** represent the core business data and rules.

**C. Standard REST API:**
The API adheres to standard HTTP verbs (GET, POST, PUT, DELETE) and utilizes correct status codes to ensure predictability for consumers.

### Architecture Layers

| Layer | Responsibilities |
|:------|:------------|
| **Controllers** | Entry point for HTTP requests. Delegates processing to Services. |
| **Services** | The "brain" of the application workflows. Orchestrates interactions between Repositories and Domain Entities and manages Transactions. |
| **Domain Entities** | Core business objects (e.g., `Project`, `ProjectOwner`) that contain both state and behavioral methods. |
| **Repositories** | Abstracts the underlying MongoDB database operations. |
| **DTOs** | Data Transfer Objects used for API contracts. Decouples the internal domain structure from the external API schema. |

### Key Implementation Details

**1. Rich Domain Models**
Entities are designed to be more than simple data holders. They enforce internal invariants through methods. For example, a `Project` entity utilizes an `EmployMember()` method to manage its internal list of members, ensuring the state remains valid during the operation.

**2. Service Layer Orchestration**
Complex business operations often require coordination across multiple domains (e.g., approving an application requires updating the Application status, the Project member list, and the User's employment history). The **Service Layer** handles this orchestration. This decision keeps the Controller layer thin and ensures that business workflows are centralized and testable.

**3. Pragmatic Repository Pattern & Transactions**
Database access is abstracted through Repository interfaces (e.g., `IProjectRepository`). However, to ensure data integrity across related documents, the repositories expose the **MongoDB Session Context**.
* **Reasoning:** This design allows the Service layer to wrap multiple repository calls into a single **ACID Transaction**. If any part of a complex workflow fails, the entire operation rolls back, preventing data inconsistency. This is a deliberate trade-off: slightly higher coupling between Service and Database Context in exchange for robust transactional safety.

**4. DTO Pattern**
The API enforces a strict separation between internal models and external contracts. Controllers exclusively accept and return **DTOs (Data Transfer Objects)**. This prevents over-posting attacks, hides internal implementation details from the client, and allows the API contract to evolve independently of the domain model.

**5. Dependency Injection**
The application leverages ASP.NET Core's built-in Dependency Injection (DI) container. All major components (Repositories, Services, Contexts) are injected via interfaces, promoting loose coupling and making the system highly testable.

---

## What I Learned

Building this architecture taught me several important concepts:

**Separation of Concerns:** Keeping UI, business logic, and data access separate makes the codebase much easier to maintain and test.

**Design Patterns:** MVVM and Repository patterns aren't just theory, they solve real problems in organizing code and making it testable.

**API Design:** Designing a clean REST API requires thinking about HTTP verbs, status codes, and how clients will consume the endpoints.

**Dependency Injection:** Once you understand DI, it makes testing and changing implementations so much easier. It's worth the initial learning curve.

**Cross-Platform Development:** Sharing business logic while adapting UI for different platforms is powerful but requires careful architectural boundaries.

All in all, I learned that software engineering requires careful planning from the start, to ensure that the application can scale well and is not difficult to maintain.

---

## Getting Started

The backend API for this project is currently **deployed and live on Azure App Service**. You do not need to run the server locally to test the application.

Follow these steps to build and run the **.NET MAUI Client** on your local machine:

### Prerequisites

  * **Visual Studio 2022** 
  * Workload installed: **.NET Multi-platform App UI development**

### Installation & Execution

1.  **Clone the repository**

    ```bash
    git clone https://github.com/cuadravan/PLinkage.git
    ```

2.  **Open the Solution**

      * Open `PLinkage.sln` in Visual Studio 2022.

3.  **Set the Startup Project**

      * In the Solution Explorer, right-click the **PLinkageApp** (MAUI) project.
      * Select **Set as Startup Project**.
      * *Note: Do not run the `PLinkageAPI` project locally unless you intend to modify backend logic. The connection string has been purposefully omitted from the repository, hence a local instance of the API will not work.*

4.  **Select Your Target Device**

      * In the standard toolbar, select your target framework (e.g., **Windows Machine** or **Android Emulator**).

5.  **Run the Application**

      * Press `F5` or click the green "Play" button.
      * The app will launch and automatically connect to the live Azure API. The MAUI app is pre-configured to consume the public Azure API endpoints. No local configuration is required.

---

## Technology Stack

**Back-end**: C#, ASP.NET Core 8, REST API, MongoDB

**Front-end**: .NET MAUI, XAML, MVVM

**Cloud & DevOps**: Azure App Service, GitHub Actions, CI/CD, Git

---

## License & Attribution

This project is licensed under the **MIT License**.

**Copyright (c) 2026 Van Kristian Cuadra**

While this code is open for educational review, the software architecture and system logic represent my professional engineering portfolio. If you are using this project as a reference for academic or professional purposes, **attribution is required**. 

Please see the [LICENSE](LICENSE) file for the full legal text.
