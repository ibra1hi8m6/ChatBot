# FullStack-ChatBot Setup Guide

To run the **FullStack-ChatBot** application, follow the steps below to set up the backend and frontend.

---

## Prerequisites

- **.NET 9 SDK** → [Download here](https://dotnet.microsoft.com/download)
- **Node.js & npm** → [Download here](https://nodejs.org/)
- **SQL Server 2022** → Ensure an instance is running and accessible.
- **Angular CLI** → Install globally:

  ```bash

npm install -g @angular/cli


## Backend Setup (ChatBot.APIs)
1. Configure the Database

Open appsettings.json in the ChatBot\ChatBot.APIs folder.

Update the ConnectionStrings section with your SQL Server 2022 connection string.

2. Run Migrations

Navigate to the backend directory:
cd ChatBot/ChatBot.Data
dotnet ef database update

3. Run the Backend
dotnet run
Start the ASP.NET Core API:
The API will typically run on:
http://localhost:7246

Frontend Setup (ChatBot-Interface)
1. Install Dependencies

Navigate to the frontend directory:
cd ChatBot-Interface
npm install

2. Configure API URL

Open the environment file (e.g., src/environments/environment.ts).

Set the apiUrl to point to your backend URL, e.g.:

apiUrl: 'http://localhost:7246'


3. Run the Frontend

Start the Angular development server:

ng serve

The application will be accessible at:
 http://localhost:4200

