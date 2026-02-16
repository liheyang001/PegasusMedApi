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
}```
4. Run ```dotnet ef database update``` in the terminal

### API endpoints
The API will be available at: http://localhost:5000
1. Client create request to target vendor - POST /api/requests
```Example Request Body:
JSON
{
  "clientId": "HOSP-001",
  "itemDetails": "200 Units of Insulin",
  "vendorIds": ["VEND-MARK", "VEND-PHARMA"],
  "requiresAcknowledgment": true
}
```