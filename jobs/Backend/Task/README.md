# Exchange Rate Provider for Czech National Bank

## **Project Overview**

This project implements an **Exchange Rate Provider** for the **Czech National Bank (CNB)**, fetching and caching exchange rates from CNB's public API.

- **Goal:** Implement a fully functional provider using a real-world public data source.
- **Data Source:** https://api.cnb.cz (CNB Exchange API)
- **Requirements:** The implementation must is **buildable, runnable**, and provide exchange rates via a testable API.

---

## **Architecture**

This solution follows the **Clean Architecture** principles, ensuring clear separation of concerns.

---

## ⚙ **Technologies Used**

- **.NET 8** - Backend framework
- **ASP.NET Core Web API** - REST API implementation
- **HttpClient with Polly** - Resilient HTTP calls
- **MemoryCache** - In-memory caching for performance
- **BackgroundService** - Periodic exchange rate updates
- **NSwag** - Auto-generated HTTP client from OpenAPI
- **FluentAssertions + NUnit + Moq** - Unit & integration testing

---

## **How to Run the Project**

### **1️⃣ Clone the Repository**

and move to jobs\Backend\Task...

### **2️⃣ Configure Settings**

Check appsettings.json in ...Task\Source\ExchangeRateProvider.Host.WebApi

```json
{
  "CnbApiOptions": {
    "BaseUrl": "https://api.cnb.cz/",
    "CacheDurationHours": 24,
    "RetryCount": 3,
    "RetryDelayMilliseconds": 500,
    "HandlerLifetimeMinutes": 5,
    "UpdateHour": 6,
    "UpdateMinute": 0
  }
}
```

### **3️⃣ Run the API**

```sh
dotnet run --project Source/ExchangeRateProvider.Host.WebApi
```

API will be available at:

```
http://localhost:5000
```

**Swagger UI for testing:**

```
http://localhost:5000/swagger
```

Note: Make sure you have dotnet installed. [Download Dotnet](https://dotnet.microsoft.com/en-us/download)

---

## **Testing**

### **Run All Tests**

```sh
dotnet test
```

## **Future Enhancements**

- Support additional data sources (other central banks, forex providers)
- Database persistence instead of memory cache
- Authentication to secure API access
- Real-time exchange rate updates using WebSockets or SignalR
