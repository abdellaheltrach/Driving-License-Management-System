# 🚗 Driving License Management System (DVLD)

The **Driving License Management System (DVLD)** is a robust, desktop-based solution designed to streamline the management of driving licenses, drivers, and related administrative processes. Built using **WinForms (C#)** and following a strict **3-Tier Architecture**, this system provides a secure and efficient platform for licensing authorities to handle the entire lifecycle of a driving license.

---

## 🌟 Key Features

### 👤 People & Driver Management
- **Centralized Profiles:** Manage personal information for applicants and drivers.
- **Search & Filter:** Advanced searching capabilities to find people or drivers quickly.
- **Driver History:** Keep track of licenses issued to each driver.

### 📝 Application Management
- **Multiple Application Types:**
  - Local Driving License Applications.
  - International Driving License Applications.
  - License Renewal.
  - Replacement for Lost or Damaged Licenses.
  - Release of Detained Licenses.
- **Workflow Tracking:** Monitor the progress of each application through various stages.

### 🧪 Test Management
- **Automated Scheduling:** Schedule Vision, Written, and Street tests.
- **Appointment Management:** Manage test appointments and prevent scheduling conflicts.
- **Test Results Tracking:** Record and store results for each test attempt.

### 🪪 License Operations
- **License Issuance:** Issue licenses upon successful completion of all tests.
- **Renewal & Replacement:** Handle license renewals and replacements for loss or damage.
- **Detain & Release:** Manage the detention of licenses and their subsequent release upon fee payment.
- **International Licenses:** Apply for and manage international driving permits.

### 🔒 User & Security
- **Authentication:** Secure login for system administrators and staff.
- **User Management:** Create, update, and manage system users and their permissions.

---

## 🏗️ Architecture

The project is built on a **3-Tier Architecture** to ensure separation of concerns, maintainability, and scalability:

1.  **Presentation Layer (DVLD):** A WinForms-based user interface that handles user interactions and displays data.
2.  **Business Logic Layer (DVLD_BusinessLayer):** Contains the core logic, validation rules, and business processes.
3.  **Data Access Layer (DVLD_DataAccessLayer):** Manages all interactions with the database, ensuring clean data retrieval and storage.

---

## 🛠️ Technology Stack

- **Language:** C#
- **Framework:** .NET Framework 4.8
- **UI:** Windows Forms (WinForms)
- **Database:** Microsoft SQL Server
- **Architecture:** 3-Tier Architecture

---

## 🚀 Getting Started

To get the project up and running locally, follow these steps:

### Prerequisites
- Visual Studio 2019 or later.
- Microsoft SQL Server.

### Setup Steps
1.  **Database Restore:**
    - Open SQL Server Management Studio (SSMS).
    - Restore the database using the provided `.bak` file located in the `DataBase & Icons` folder.
2.  **Configuration:**
    - Open the solution in Visual Studio.
    - Navigate to the `DVLD_DataAccessLayer` project.
    - Open `clsDataAccessSettings.cs` and update the `ConnectionString` with your SQL Server credentials.
3.  **Run:**
    - Build the solution.
    - Set `DVLD` as the startup project and press `F5`.

---

## 📂 Project Structure

- `DVLD/`: The presentation layer containing all Forms and User Controls.
- `DVLD_BusinessLayer/`: The logic layer containing business objects and rules.
- `DVLD_DataAccessLayer/`: The data layer containing SQL queries and database logic.
- `DataBase & Icons/`: Contains the database backup (`.bak`) and project assets.

---

## 🤝 Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request if you have suggestions or improvements.

---

## 📄 License

This project is open-source. Please credit the author if used in other projects.
