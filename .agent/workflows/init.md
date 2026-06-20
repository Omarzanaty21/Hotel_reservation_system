---
description: Initialize project context - explore codebase and summarize architecture
---

# Init Workflow

Run this workflow to (re)familiarize with the Hotel Reservation System codebase.

## 1. Review Project Overview

Read the project map at `.agent/workflows/PROJECT_MAP.md` for a full architectural overview including tech stack, folder structure, entities, endpoints, and dev commands.

## 2. Start the Backend (API)

// turbo

```bash
cd API && dotnet run --project src/HotelReservation.API
```

The API runs at **http://localhost:5026** with Swagger at `/swagger`.

## 3. Start the Frontend (Presentation)

// turbo

```bash
cd Presentation && npm start
```

The Angular app runs at **http://localhost:4200**.

## 4. Verify Connectivity

Open http://localhost:4200 in browser and confirm the rooms page loads data from the API.
