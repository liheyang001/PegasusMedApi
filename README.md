# Pegasus Medical  API

### Prerequisites
* .NET 10 SDK
* PostgreSQL
* Node.js
* Angular CLI (npm install -g @angular/cli)

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
5. Run ```dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update```

### Frontend Setup (Angular)
1. Navigate to the client folder: cd PegasusMedApi.Client
2. Install dependencies: npm install
3. Start the dev server: ng serve
4. Open your browser to http://localhost:4200

### API endpoints
The API will be available at: http://localhost:5000
Swagger is available at: http://localhost:5000/swagger/v1/swagger.json

1. Retrieves all medical supply requests - GET /api/requests
```Example Response Body:
JSON
[
    {
        "id": 25,
        "clientId": "0",
        "itemDetails": "",
        "createdAt": "2026-02-17T10:40:02.787537Z",
        "vendorAssignments": []
    },
    {
        "id": 24,
        "clientId": "1",
        "itemDetails": "sdweqw",
        "createdAt": "2026-02-17T10:38:04.988068Z",
        "vendorAssignments": [
            {
                "id": 58,
                "medRequestId": 24,
                "vendorId": "dsdd",
                "isFlagged": true,
                "acknowledgedAt": "2026-02-17T11:03:31.852375Z"
            }
        ]
    }
] 
```
2. Client create request to target vendor - POST /api/requests
```Example Request Body:
JSON
{
  "clientId": "HOSP-001",
  "itemDetails": "200 Units of Insulin",
  "vendorIds": ["VEND-001", "VEND-002"],
  "isFlagged": true
}
```
3. Client request details by ID - GET /api/requests/{id}
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
4. Vendor retrieve all requests by VendorID - Get /api/requests/vendor/{vendorId}
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
5. Vendor update IsFlagged Status - PATCH /api/requests/{id}/vendor/{vendorId}/acknowledge
```Example Response Body:
JSON
{
    "message": "Update successful"
}
```
6. Get All Vendors - GET /api/requests/vendors
```Example Response Body:
JSON
[
    "VEND-001",
    "VEND-002",
    "VEND-003"
]
```