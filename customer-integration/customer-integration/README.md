# 🏠 Customer Service Workflow – Stage 1 (Azure Durable Functions)

This project implements a **customer service workflow for property management** using Azure Durable Functions.  
The goal is to handle a service case from creation to appointment scheduling and initial customer notification.

---

# 🚀 Overview

When a customer reports an issue (e.g. heating failure in an apartment), the system automatically:

1. Creates a service case in the system
2. Determines priority based on the issue
3. Creates a service appointment for a technician
4. Sends a confirmation email to the customer

All steps are orchestrated using **Azure Durable Functions**, ensuring reliable execution, state management, and observability.

---

# 🧩 Architecture (Stage 1 Flow)

HTTP Trigger
↓
Orchestrator Function
↓
Create Ticket Activity
↓
Set Priority Activity
↓
Create Appointment Activity
↓
Send Email Activity

---

# ⚙️ Components

## 1. HTTP Starter Function

Receives a request from external systems (e.g. Power Automate, CRM, Postman).

- Accepts JSON payload with case details
- Starts a new orchestration instance
- Returns a status tracking URL

Example input:

```json
{
  "title": "No heating",
  "customer": "John Doe",
  "apartment": "A-45",
  "description": "The apartment has no heating",
  "status": "New"
}
```

2. Orchestrator Function

The orchestrator controls the entire workflow:

Responsibilities:

Maintains workflow state using a CaseState object
Calls activity functions in sequence
Passes data between steps
Ensures deterministic execution

Flow:

Create case
Set priority
Create appointment
Send email notification 3. CreateTicketActivity

Creates a service case in the system (e.g. CRM or database).

Responsibilities:

Stores case details
Returns generated CaseId 4. SetPriorityActivity

Determines the priority of the case based on business rules.

Example logic:

Heating failure → HIGH priority
Minor cosmetic issue → LOW priority 5. CreateAppointmentActivity

Schedules a technician visit.

Responsibilities:

Assigns a technician
Sets appointment date based on priority
Returns AppointmentId 6. SendEmailActivity

Sends a confirmation email to the customer.

Responsibilities:

Uses case and appointment data
Sends notification via email provider or CRM template system
Confirms that the request has been registered

🔐 Technical Notes
Uses JsonSerializerOptions.PropertyNameCaseInsensitive = true for flexible JSON input (title vs Title)
State is maintained using a CaseState object passed between activities
Orchestrator is replay-safe and deterministic
Logging uses CreateReplaySafeLogger

📦 Benefits of This Approach
Fully serverless architecture
Reliable execution with retries and state tracking
Clear separation of concerns (each activity has a single responsibility)
Easy integration with external systems (CRM, Power Automate, APIs)
Observability via Azure Durable Functions instance tracking
