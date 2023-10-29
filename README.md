# PV179 - Bookhub

- **Name**: Bookhub
- **Techstack**
   - **Backend**
      - C# (ASP.NET Core for the API)
      - Entity Framework Core for data access and migrations
      - SQLite Database
      - Swagger for API documentation
   - **Frontend** _(TODO)_
   - **Development tools**
      - Package Manager : NuGet
      - Version Control : Git
- **Team Leader**
   - Oliver Šintaj
- **Developers**
  - Pavol Baran
  - Jozef Mihale
  - Samuel Líška
- **Assignment :** Develop a digital platform for the company called "BookHub", a company that sells books of various genres. The platform should facilitate easy browsing and purchase of books, letting customers sort and filter by authors, publishers, and genres. After customers create accounts, they should be able to review their purchase history, rate books, and make wishlists. Administrators should have privileges to update book details, manage user accounts, and regulate book prices.


## Table of Contents

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [Folder Structure](#folder-structure)

## Prerequisites

1. **.NET 7.0 Runtime:**
   Ensure that you have the .NET 7.0 Runtime installed on your development machine. You can download it from the official [.NET website](https://dotnet.microsoft.com/download).

2. **Integrated Development Environment (IDE):**
   You will need an Integrated Development Environment (IDE) for .NET development. We recommend using Visual Studio as it provides a comprehensive set of tools for .NET development.

3. **Visual Studio Installer Workloads:**
   In Visual Studio, make sure to select the following workloads using the Visual Studio Installer:
     - "ASP .Net and web development."

4. **Docker: (Not yet implemented)**
  Install Docker to support containerized development and deployment.

## Project Tasks

### Milestone 1

1. **Design & Documentation**:
   - Make a use-case diagram and data model for the application.
   - Create a documentation Technical Overview (can be part of the readme) with the diagrams and information about the application.

2. **API & Backend**:
   - Create a REST web API capable of fetching products based on their 'name', 'description', 'price', 'Category', and 'Manufacturer'.
   - Setup the database and introduce a DAL (Data Access Layer) to your project.
   - Seed the database with real-looking data. Avoid using placeholders like 'AAAA', 'test', etc.
   - Create an Authentication Middleware (a simple middleware using only a hard-coded access token is acceptable).
   - Implement a Logging middleware that logs all incoming requests.

3. **Version Control & Collaboration**:
   - Set up a GitLab repository and set its visibility to Internal (not Private).
   - Divide the work between the Team Lead and Team members.
   - Each team member must contribute equally. Every member should:
     - Create a database entity.
     - Seed the created entity.
     - Create CRUD operations (Web API Endpoints) for the created entity.
   - All changes should be committed to separate branches and merged via merge requests.
     - At least one approval from a team member is required for merge requests.
     - The team member who approves a merge request is also responsible for the changes. This rule starts from the 2nd milestone.
   - All merge requests for milestone 1 should be merged into a branch named 'Milestone 1'. Once done with the milestone, initiate a merge request from 'Milestone 1' to 'master'. This allows reviewers to see all changes in one place. The 'Milestone 1' branch can be merged only after teacher approval.

4. **Documentation & Onboarding**:
   - Create a README that provides basic information about how to run the application, its components, etc.

### Milestone 2

### Milestone 3

## Installation

### Step 1: Install .NET 7.0 Runtime

1. Visit the official [.NET website](https://dotnet.microsoft.com/download).

2. Download the .NET 7.0 Runtime for your operating system (Windows, macOS, or Linux).

3. Follow the installation instructions to install the .NET Runtime on your machine.

### Step 2: Install an Integrated Development Environment (reccomended IDE is Visual Studio)

1. Download and install Visual Studio from the official website: [Visual Studio](https://visualstudio.microsoft.com/).

2. During installation, make sure to select the appropriate workloads, such as "ASP .Net and web development," using the Visual Studio Installer.

### Step 3: Install Docker

1. Download Docker from the official website based on your operating system:
   - [Docker for Windows](https://docs.docker.com/desktop/install/windows-install/)
   - [Docker for macOS](https://docs.docker.com/desktop/install/mac-install/)
   - [Docker for Linux](https://docs.docker.com/desktop/install/linux-install/)

2. Follow the installation instructions for your specific operating system to install Docker.


## Usage

Follow these steps to use the project effectively:

### Clone the Repository

```shell
git clone https://gitlab.fi.muni.cz/xbaran4/bookhub.git
```
### Update Databse based on migration
Before running the program itself, use the DAL.SQLite.Migrations project to update the databse.  
When running with Visual Studio, open PMC (`Tools -> Nuget Package Manager -> Package Manager Console`).
```shell
# update databse based on existing snapshot(s)
Update-Database -project  DAL.SQLite.Migrations

# or make changes to entities and create a new migration
Add-Migration <migration_name> -project DAL.SQLite.Migrations
# when using the newly created migration, do not forget to update the databse
```
### Run the project
```shell
# Navigate to the project directory
cd <project_directory>

# Install project dependencies
dotnet restore

# Build the application
dotnet build

# Run the application
dotnet run
```
This will start your project. Access the application using a web browser at http://localhost:5000 (or the appropriate address).

## Folder Structure

The project is organized into several key folders to maintain a clean and organized codebase. Here's an overview of the main folders:

### BookhubWeAPI

- The presentation layer where API endpoints, controllers, mappers, and middleware are located.

### DataAccessLayer

- Handles database access, models, and related data operations.

### Infrastructure

- Contains repositories and follows the Unit of Work pattern for managing data operations.

### DAL.SQLite.Migrations

- A dedicated project for SQLite database migrations to manage changes to the database schema.

## Technical overview

### Dependencies

- **[.NET 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)**
   - [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-7.0)
   - [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
   - [Swashbuckle](https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=visual-studio)

- **[SQLite](https://www.sqlite.org/index.html)**
   - [Microsoft.EntityFrameworkCore.Sqlite](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/?tabs=dotnet-core-cli)


### Use-Case Diagram

![My UML Diagram](http://cdn-0.plantuml.com/plantuml/png/TLBTRXDD3BsVf_3Nctv8JNW1X5gAK96ea0A56wJ8cN5thSnFDFOage1tPtPcexqWxCNAnpuU-zY-5SMi9NXp3KUMWXw4L3aEOlxJaGA1fLmyOLobefuZcP5TzNAsdepiEHumOu0TslsGKudkFlcKuJIoaZ6UdXKqGUPXL72SoIgdQ0nQhQmx5pW22g0Bi-iH8mwK9sSOyknyakufaesfFtZpQxLgyJ1qq5wetrG0u-09tgMqVoWx-6bYKeAh3oc3S3XujgmlDt0TMgLhi1XXHs1J57QKoS6FrfCeHeVPWP12UeRxpTDxI0VAE4cGzTL4YZerH1Q5uFzFQG2EhrFHLqrKqN49Dc6zBVYYAPCqqkDM9l0Nxu455YvyZ4UArNgPapWUBwRSMPjAr9c5rbvaVSwEyhb-okuYwJlBw5dqp3jLl12_CPrQWpsysUwg_MyL6snNrRceT9c-lFVO2R_DlBsl8ytNmWBeVJgH0qtG4y-ovQRkhgkh9gbTJujBEbA_ePeQNSzfGE14tlTDeWlRgPQf1gpN2su7jrgzdVVPGH_kWleKErfcrU0ocmuN-QrKJ-X7iE0U3qDFwVk-fUXAy7y0)

### ERD Diagram

![My UML Diagram](http://cdn-0.plantuml.com/plantuml/png/jPNFQXin4CRlUWerbq8XVe0GGjf28VHGQ2azBkFLu1NMIkxCoAx9vjqhUUEDDjw53QMvvVlcpy_ZqVYU1KXYwzHxlRSSmMb64UithDv9Wotg6Yaw17yTeZZhKJNM9DLIxL3nrleT4BGwMtBSeGuaJIYCDTJR3ONepUVW0kb3OmLL-aClLexwwYSZNUb7fVK7Q_IDzh5T8-dRsmrw910qTtV9lBxE4P6HFBIOuWJ_I9Psm7m8PAe6k2bq1YV5lnGStcXCrTNJI_DVbXjdMTu4K8t3ldnDVh9gmXnSGMvhG51iYzamo3NPdTZWpq0J20-2xMACmtFI3CewXEsCjICRA3U-7COtCaZ_TH8I15oL46eSyhBE0e9pH6yQJCWPowTIMYo1S6vgaUe662VOVqVeQGUn-02VKjt5x2d5bPiDKPf0XM1vxSlDZsjdkS4o9DMbOPBJkzSlGOrplDznR_6mc7f-gdChCiz5a6zFkGW5rmy9X8knm1X2vfdsBNZOpCAzlZ1Dy5ZrKOMCBLWtyQ0_zfqNax8GeXI5U-6oOPtBbs4W8yoG55VLmU04rOFVevT0nyNFwH-VZFLLikljwB4uOAa8JqPn6X9d_XdGNRTQXKTzFeaZyNGBHihBiXPofuTTKUfSw7NXF0EL2uwhdsILyoTQb79PfpJACWE6xi9grNNwUIyAup9fz2tffpvnI9YtrJrw4rlt5m00)
