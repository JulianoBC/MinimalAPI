# Explanation of PersonalKnowledgeBase.Backend/Program.cs

This document provides a detailed explanation of the `PersonalKnowledgeBase.Backend/Program.cs` file, which defines the backend API using .NET Minimal APIs.

```csharp
using PersonalKnowledgeBase.Backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4321")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");


var notes = new List<Note>();

app.MapGet("/api/notes", () =>
{
    return notes;
})
.WithName("GetNotes")
.WithOpenApi();

app.MapGet("/api/notes/{id}", (int id) =>
{
    var note = notes.Find(n => n.Id == id);
    if (note == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(note);
})
.WithName("GetNoteById")
.WithOpenApi();

app.MapPost("/api/notes", (Note note) =>
{
    note.Id = notes.Count + 1;
    notes.Add(note);
    return Results.Created($"/api/notes/{note.Id}", note);
})
.WithName("CreateNote")
.WithOpenApi();

app.MapPut("/api/notes/{id}", (int id, Note inputNote) =>
{
    var note = notes.Find(n => n.Id == id);
    if (note == null)
    {
        return Results.NotFound();
    }
    note.Title = inputNote.Title ?? note.Title; // Use ?? to keep existing value if input is null
    note.Content = inputNote.Content ?? note.Content;
    note.UpdatedDate = DateTime.UtcNow;
    return Results.NoContent();
})
.WithName("UpdateNote")
.WithOpenApi();

app.MapDelete("/api/notes/{id}", (int id) =>
{
    var note = notes.Find(n => n.Id == id);
    if (note == null)
    {
        return Results.NotFound();
    }
    notes.Remove(note);
    return Results.NoContent();
})
.WithName("DeleteNote")
.WithOpenApi();


app.Run();
```

## 1. Namespace and Model

*   `using PersonalKnowledgeBase.Backend.Models;`: This line imports the `Models` namespace from the `PersonalKnowledgeBase.Backend` project. This namespace contains the `Note` model class, which defines the structure of a note object.

## 2. WebApplication Builder

*   `var builder = WebApplication.CreateBuilder(args);`: This line creates a `WebApplicationBuilder`, which is essential for configuring and building the web application. It takes command-line arguments (`args`) as input, allowing for configuration through command-line parameters.

## 3. Service Configuration

This section configures services that will be used by the application. Services are components that provide functionalities like API exploration, documentation, and Cross-Origin Resource Sharing (CORS).

*   `builder.Services.AddEndpointsApiExplorer();`:  This service enables API endpoint discovery. It's required for tools like Swagger to understand the API structure.
*   `builder.Services.AddSwaggerGen();`: This service adds Swagger (OpenAPI) generation capabilities. Swagger is used to automatically create API documentation and a UI for testing the API.
*   `builder.Services.AddCors(options => { ... });`: This configures Cross-Origin Resource Sharing (CORS). CORS is a security feature implemented by browsers to restrict web pages from making requests to a different domain than the one that served the web page.
    *   `options.AddPolicy("AllowSpecificOrigin", builder => { ... });`:  This adds a CORS policy named "AllowSpecificOrigin".
    *   `builder.WithOrigins("http://localhost:4321")`: This specifies that requests from `http://localhost:4321` (where the frontend is running) are allowed.
    *   `.AllowAnyMethod()`: Allows any HTTP method (GET, POST, PUT, DELETE, etc.) from the allowed origin.
    *   `.AllowAnyHeader()`: Allows any HTTP headers from the allowed origin.

## 4. Building the WebApplication

*   `var app = builder.Build();`: This line builds the `WebApplication` instance using the configurations set up in the `builder`. The `app` variable now represents the fully configured web application.

## 5. HTTP Request Pipeline Configuration (Middleware)

This section configures the HTTP request pipeline, which defines how the application handles incoming HTTP requests. Middleware components are added to this pipeline to handle various aspects of request processing.

*   `if (app.Environment.IsDevelopment()) { ... }`: This conditional block executes only when the application is running in a development environment.
    *   `app.UseSwagger();`:  Adds the Swagger middleware to the pipeline. This middleware generates the Swagger JSON document.
    *   `app.UseSwaggerUI();`: Adds the Swagger UI middleware, which provides a user interface to interact with and test the API documentation generated by Swagger.
*   `app.UseHttpsRedirection();`: This middleware automatically redirects HTTP requests to HTTPS. In development, this might not be strictly necessary but is good practice for production.
*   `app.UseCors("AllowSpecificOrigin");`:  Adds the CORS middleware to the pipeline, applying the "AllowSpecificOrigin" policy defined earlier. This ensures that CORS checks are performed for incoming requests.

## 6. In-Memory Data Store

*   `var notes = new List<Note>();`: This line creates a simple in-memory list of `Note` objects. In this basic example, notes are stored in memory and will be lost when the application restarts. For a persistent application, a database would be used instead.

## 7. API Endpoints Mapping

This section defines the API endpoints using Minimal APIs. Each `app.Map...` method defines a specific endpoint and its handler function.

*   **`app.MapGet("/api/notes", ...)`**: Defines a GET endpoint at `/api/notes` to retrieve all notes.
    *   `() => { return notes; }`: The handler function simply returns the `notes` list.
    *   `.WithName("GetNotes")`: Assigns the name "GetNotes" to this endpoint, which can be used for route generation and linking.
    *   `.WithOpenApi()`: Configures OpenAPI metadata for this endpoint, allowing Swagger to document it.

*   **`app.MapGet("/api/notes/{id}", ...)`**: Defines a GET endpoint at `/api/notes/{id}` to retrieve a note by its ID.
    *   `(int id) => { ... }`: The handler function takes an integer `id` from the route.
    *   `var note = notes.Find(n => n.Id == id);`:  Finds a note in the `notes` list with the matching `id`.
    *   `if (note == null) { return Results.NotFound(); }`: If no note is found, returns a 404 Not Found response.
    *   `return Results.Ok(note);`: If a note is found, returns a 200 OK response with the note object.
    *   `.WithName("GetNoteById").WithOpenApi()`: Configures name and OpenAPI for this endpoint.

*   **`app.MapPost("/api/notes", ...)`**: Defines a POST endpoint at `/api/notes` to create a new note.
    *   `(Note note) => { ... }`: The handler function takes a `Note` object from the request body. Minimal APIs automatically deserialize JSON request bodies to objects.
    *   `note.Id = notes.Count + 1;`: Assigns a new ID to the note (simple incrementing ID for in-memory list).
    *   `notes.Add(note);`: Adds the new note to the `notes` list.
    *   `return Results.Created($"/api/notes/{note.Id}", note);`: Returns a 201 Created response with the URI of the new note in the `Location` header and the created note in the response body.
    *   `.WithName("CreateNote").WithOpenApi()`: Configures name and OpenAPI.

*   **`app.MapPut("/api/notes/{id}", ...)`**: Defines a PUT endpoint at `/api/notes/{id}` to update an existing note.
    *   `(int id, Note inputNote) => { ... }`: Takes an `id` from the route and an `inputNote` object from the request body.
    *   `var note = notes.Find(n => n.Id == id);`: Finds the note to update.
    *   `if (note == null) { return Results.NotFound(); }`: Returns 404 if note not found.
    *   `note.Title = inputNote.Title ?? note.Title;`: Updates the note's `Title` with the new title from `inputNote` if provided, otherwise keeps the existing title (using null-coalescing operator `??`).
    *   `note.Content = inputNote.Content ?? note.Content;`: Updates the `Content` similarly.
    *   `note.UpdatedDate = DateTime.UtcNow;`: Updates the `UpdatedDate` to the current UTC time.
    *   `return Results.NoContent();`: Returns a 204 No Content response to indicate successful update without returning content in the body.
    *   `.WithName("UpdateNote").WithOpenApi()`: Configures name and OpenAPI.

*   **`app.MapDelete("/api/notes/{id}", ...)`**: Defines a DELETE endpoint at `/api/notes/{id}` to delete a note.
    *   `(int id) => { ... }`: Takes an `id` from the route.
    *   `var note = notes.Find(n => n.Id == id);`: Finds the note to delete.
    *   `if (note == null) { return Results.NotFound(); }`: Returns 404 if note not found.
    *   `notes.Remove(note);`: Removes the note from the `notes` list.
    *   `return Results.NoContent();`: Returns a 204 No Content response to indicate successful deletion.
    *   `.WithName("DeleteNote").WithOpenApi()`: Configures name and OpenAPI.

## 8. Running the Application

*   `app.Run();`: This line starts the web application and begins listening for incoming HTTP requests. The application will run until it is manually stopped (e.g., by pressing Ctrl+C in the console).

This explanation should provide a good understanding of the `PersonalKnowledgeBase.Backend/Program.cs` file and how it sets up a basic RESTful API using .NET Minimal APIs.