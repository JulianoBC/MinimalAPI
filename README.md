# Personal Knowledge Base

## Project Description

Personal Knowledge Base is a web application designed to help users capture, organize, and retrieve personal notes and information efficiently. It consists of a Minimal API backend built with .NET 8 and a user-friendly frontend developed using AstroJS.

## Technology Stack

**Backend:**

*   .NET 8 Minimal API (C#)

**Frontend:**

*   AstroJS (TypeScript)

## Setup Instructions

### Backend Setup (.NET Minimal API)

1.  **Install .NET 8 SDK:**  [Download .NET SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2.  **Navigate to the backend directory:**
    ```bash
    cd PersonalKnowledgeBase.Backend
    ```
3.  **Run the backend:**
    ```bash
    dotnet run
    ```
    The backend will start at `http://localhost:5274`.

### Frontend Setup (AstroJS)

1.  **Install Node.js and npm:** [Download Node.js](https://nodejs.org/)
2.  **Navigate to the frontend directory:**
    ```bash
    cd PersonalKnowledgeBase.Frontend
    ```
3.  **Install dependencies:**
    ```bash
    npm install
    ```
4.  **Run the frontend development server:**
    ```bash
    npm run dev
    ```
    The frontend will be accessible at `http://localhost:4321`.

## How to Run the Application

1.  **Start the Backend:** Open a terminal, navigate to `PersonalKnowledgeBase.Backend`, and run `dotnet run`.
2.  **Start the Frontend:** Open another terminal, navigate to `PersonalKnowledgeBase.Frontend`, and run `npm run dev`.
3.  **Access the Application:** Open your browser and go to `http://localhost:4321`.

## API Endpoints

The backend provides the following API endpoints:

*   `GET /api/notes`:  Retrieve all notes.
*   `GET /api/notes/{id}`: Retrieve a note by ID.
*   `POST /api/notes`: Create a new note.
*   `PUT /api/notes/{id}`: Update a note.
*   `DELETE /api/notes/{id}`: Delete a note.

For more details, refer to the `PROJECT_DOCUMENTATION.md` file.