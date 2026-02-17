# Pegasus Medical  API

### Prerequisites
* .NET 10 SDK
* PostgreSQL
* Node.js
* Angular

### Installation
1. Clone the repo: `git clone https://github.com/liheyang001/PegasusMedApi.git`

### PostgreSQL Setup
To run this project locally, you need to set up the PostgreSQL Database
1. Install the Entity Framework Core tool by run ```dotnet tool install --global dotnet-ef```
2. Create the database by Open pgAdmin4, Right-click Databases > Create > Database. Name the database PegasusMedDb.
3. Open appsettings.json and update the DefaultConnection with your PostgreSQL, add ```"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=PegasusMedDb;Username=postgres;Password=1234"
4. Update the `Password` field (currently `1234`) to match your local PostgreSQL password.
}```
5. Run ```dotnet ef database update``` in the terminal

### API endpoints
The API will be available at: http://localhost:5000
1. Client create request to target vendor - POST /api/requests
```Example Request Body:
JSON
{
  "clientId": "HOSP-001",
  "itemDetails": "200 Units of Insulin",
  "vendorIds": ["VEND-001", "VEND-002"],
  "isFlagged": true
}
```
2. Client request details by ID - GET /api/requests/{id}
```Example Response Body:
JSON
{
  "id": 15,
  "clientId": "HOSP-001",
  "itemDetails": "200 Units of Insulin",
  "createdAt": "2026-02-17T20:30:00Z",
  "vendorAssignments": [
    {
      "vendorId": "VEND-001",
      "isFlagged": true,
      "acknowledgedAt": "2026-02-17T21:00:00Z"
    }
  ]
}
```
3. Vendor retrieve all requests by VendorID - Get /api/requests/vendor/{vendorId}
```Example Response Body:
JSON
[
    {
        "id": 1,
        "clientId": "3",
        "itemDetails": "50x Heart Rate Monitors",
        "createdAt": "2026-02-16T12:09:10.391113Z",
        "isFlagged": true
    },
    {
        "id": 2,
        "clientId": "3",
        "itemDetails": "50x Heart Rate Monitors",
        "createdAt": "2026-02-16T12:21:38.059258Z",
        "isFlagged": true
    }
]
```
4. Vendor update IsFlagged Status - PATCH /api/requests/{id}/vendor/{vendorId}/acknowledge
```Example Response Body:
JSON
{
    "message": "Update successful"
}
```