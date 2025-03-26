// Import the Note model
using PersonalKnowledgeBase.Backend.Models;

// Create a WebApplication builder, which is essential for configuring and building the web application.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Services are components that provide functionalities like API exploration, documentation, and CORS.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // Enables API endpoint discovery, required for tools like Swagger.
builder.Services.AddSwaggerGen(); // Adds Swagger (OpenAPI) generation capabilities for API documentation and testing UI.
builder.Services.AddCors(options => // Configures Cross-Origin Resource Sharing (CORS) to allow frontend access.
{
    options.AddPolicy("AllowSpecificOrigin",  // Adds a CORS policy named "AllowSpecificOrigin".
        builder =>
        {
            builder.WithOrigins("http://localhost:4321") // Specifies that requests from http://localhost:4321 (frontend) are allowed.
                   .AllowAnyMethod() // Allows any HTTP method (GET, POST, PUT, DELETE, etc.).
                   .AllowAnyHeader(); // Allows any HTTP headers.
        });
});


var app = builder.Build(); // Build the WebApplication instance with the configured services.

// Configure the HTTP request pipeline (middleware).
if (app.Environment.IsDevelopment()) // Conditional block executes only in development environment.
{
    app.UseSwagger(); // Adds Swagger middleware to generate OpenAPI JSON document.
    app.UseSwaggerUI(); // Adds Swagger UI middleware to provide a UI for API documentation and testing.
}

app.UseHttpsRedirection(); // Middleware to redirect HTTP requests to HTTPS.
app.UseCors("AllowSpecificOrigin"); // Adds CORS middleware to apply the "AllowSpecificOrigin" policy.

// In-memory list to store notes (for demonstration purposes; data is lost on restart).
var notes = new List<Note>();

// Map GET request to /api/notes endpoint to retrieve all notes.
app.MapGet("/api/notes", () =>
{
    return notes; // Handler function returns the list of notes.
})
.WithName("GetNotes") // Assigns the name "GetNotes" to the endpoint for route generation.
.WithOpenApi(); // Configures OpenAPI metadata for Swagger documentation.

// Map GET request to /api/notes/{id} to retrieve a note by its ID.
app.MapGet("/api/notes/{id}", (int id) =>
{
    var note = notes.Find(n => n.Id == id); // Finds a note in the list with a matching ID.
    if (note == null)
    {
        return Results.NotFound(); // Returns 404 Not Found if no note with the given ID is found.
    }
    return Results.Ok(note); // Returns 200 OK with the found note object.
})
.WithName("GetNoteById") // Assigns the name "GetNoteById" to this endpoint.
.WithOpenApi(); // Configures OpenAPI metadata for Swagger.

// Map POST request to /api/notes to create a new note.
app.MapPost("/api/notes", (Note note) =>
{
    note.Id = notes.Count + 1; // Assigns a new ID to the note (simple incrementing ID for in-memory list).
    notes.Add(note); // Adds the new note to the in-memory list.
    return Results.Created($"/api/notes/{note.Id}", note); // Returns 201 Created response, including the new note's URI and object.
})
.WithName("CreateNote") // Assigns the name "CreateNote" to the endpoint.
.WithOpenApi(); // Configures OpenAPI metadata for Swagger.

// Map PUT request to /api/notes/{id} to update an existing note.
app.MapPut("/api/notes/{id}", (int id, Note inputNote) =>
{
    var note = notes.Find(n => n.Id == id); // Finds the note to be updated.
    if (note == null)
    {
        return Results.NotFound(); // Returns 404 Not Found if the note is not found.
    }
    note.Title = inputNote.Title ?? note.Title; // Updates the note's Title, using existing title if new title is null.
    note.Content = inputNote.Content ?? note.Content; // Updates the note's Content, using existing content if new content is null.
    note.UpdatedDate = DateTime.UtcNow; // Updates the UpdatedDate to the current UTC time.
    return Results.NoContent(); // Returns 204 No Content for successful update (no response body).
})
.WithName("UpdateNote") // Assigns the name "UpdateNote" to the endpoint.
.WithOpenApi(); // Configures OpenAPI metadata for Swagger.

// Map DELETE request to /api/notes/{id} to delete a note.
app.MapDelete("/api/notes/{id}", (int id) =>
{
    var note = notes.Find(n => n.Id == id); // Finds the note to delete.
    if (note == null)
    {
        return Results.NotFound(); // Returns 404 Not Found if the note is not found.
    }
    notes.Remove(note); // Removes the note from the in-memory list.
    return Results.NoContent(); // Returns 204 No Content for successful deletion.
})
.WithName("DeleteNote") // Assigns the name "DeleteNote" to the endpoint.
.WithOpenApi(); // Configures OpenAPI metadata for Swagger.


app.Run(); // Runs the web application, listening for HTTP requests.
