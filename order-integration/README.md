# 🚀 Azure Serverless Integration System

## 📌 Overview

This project demonstrates a **production-style serverless integration architecture** built with:

* Azure Functions (Isolated Worker)
* Durable Functions (orchestration)
* Service Bus (event-driven communication)
* CRM (Dataverse)
* SharePoint + Power Automate
* SQL Database
* Azure AI (Form Recognizer)

The system processes documents (e.g., orders/invoices) and executes a **fully automated business workflow**.

---

## 🧩 Architecture

The solution follows **Clean Architecture principles**:

```
Function → Orchestrator → Activities → Domain → Infrastructure
```

### Layers:

* **Functions**

  * Entry points (Service Bus / HTTP triggers)
  * Start orchestration

* **Orchestrators**

  * Control workflow execution order
  * Handle retries and failures

* **Activities**

  * Execute single units of work
  * Call domain services

* **Domain**

  * Business logic (pure C#)
  * No external dependencies

* **Infrastructure**

  * Integrations (CRM, SQL, Service Bus, AI)

---

## 🔄 End-to-End Workflow

```
SharePoint (file upload)
   ↓
Power Automate
   ↓
Service Bus (orders queue)
   ↓
Azure Function (starter)
   ↓
Durable Orchestrator
   ↓
Activities:
   1. ProcessDocument (AI extraction)
   2. CreateSeller (CRM)
   2. CreateVendor (CRM)
   3. CreateInvoice (CRM)
   4. CreateProducts (CRM)
   5. CreateOrder (CRM)
   ↓
On failure:
   ↓
Service Bus (notifications queue)
   ↓
Power Automate → Email notification
```

---

## ⚙️ Key Features

### ✅ Event-driven architecture

* Decoupled components using Service Bus

### ✅ Durable workflows

* Guaranteed execution order
* Built-in retry policies

### ✅ Clean separation of concerns

* Domain logic isolated from infrastructure

### ✅ External integrations

* CRM (Dataverse)
* SharePoint
* SQL Database

### ✅ AI document processing

* Extract structured data from PDFs using Azure AI

### ✅ Failure handling & observability

* Application Insights logging
* Dead-letter queues
* Notification system via Power Automate

---

## 🧠 Design Decisions

* **Durable Functions** used for orchestration instead of chaining functions manually
* **Service Bus** for decoupling and reliability
* **Interfaces (ICrmClient, IRepository)** for testability
* **Mapping layer** between domain and CRM models
* **Retry policies** for resilience

---

## 🧪 Testing Strategy

* Unit tests using:

  * xUnit
  * Moq
* Mocked external dependencies (CRM, database)
* Focus on testing business logic in Domain Services

---

## 🔐 Configuration & Security

* Secrets stored in Azure Key Vault
* Managed Identity for secure access
* No hardcoded credentials

---

## 📂 Project Structure

```
/src
  /Functions
  /Orchestrators
  /Activities
  /Domain
  /Application
  /Infrastructure
  /Common
```

---

## 🚀 Technologies

* .NET (C#)
* Azure Functions (Isolated)
* Durable Functions
* Azure Service Bus
* Azure SQL
* Azure AI (Form Recognizer)
* Dataverse (CRM)
* Power Automate

---

## 📈 Future Improvements

* Idempotency handling
* Distributed tracing (correlation IDs)
* Advanced monitoring dashboards
* CI/CD pipeline (GitHub Actions / Azure DevOps)

---

## 👨‍💻 Author

This project is designed to demonstrate **modern Azure integration patterns and cloud architecture skills**.
