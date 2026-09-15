**KnitApp**

A web app for managing knitting and crochet patterns. Built as a personal project to learn full-stack development with Blazor and .NET.

**Features**
- Add, edit, and delete patterns (name, description, instructions, craft type)
- Add materials (yarn) and equipment (needles, notions) to each pattern
- Yarn name autocomplete, backed by a searchable yarn catalog
- Upload photos and a PDF with instructions for each pattern
- Generate a shopping list from several patterns, combining identical yarn types
- A REST API for patterns (/api/patterns), with Swagger docs

**Tech stack**
- Blazor Server (.NET 10)
- Entity Framework Core + SQLite
- xUnit for testing
- Swashbuckle for Swagger/OpenAPI

**How it's built**
- Each feature (patterns, images, PDFs, shopping list, yarn catalog) has its own service behind an interface, injected with DI — no manual new() of services anywhere.
- Razor pages are split into a markup file (.razor) and a code file (.razor.cs), so the C# logic isn't mixed in with the HTML.
- /api/patterns is a normal REST API (GET, GET by id, POST, PUT, DELETE) built with minimal APIs, separate from the Blazor pages (which still talk to the services directly).
- File uploads (PDFs, images) are handled by services on the backend, not directly in the Razor components.
- Tests use xUnit. Services with no database (like the shopping list logic) get plain unit tests. Services that use the database are tested against a real in-memory SQLite database, not a mock.

**API endpoints***
| Method | Route | What it does|
|---|---|---|
|GET|	/api/patterns|	List all patterns|
|GET|	/api/patterns/{id}|	Get one pattern|
|POST|	/api/patterns|	Create a pattern|
|PUT|	/api/patterns/{id}|	Update a pattern|
|DELETE|	/api/patterns/{id}|	Delete a pattern|
|POST|	/api/patterns/pdf|	Upload a PDF, get back the file path|

Swagger UI is available at /swagger when running locally.

**How I work on this project**
- One GitHub issue per task, one branch per issue (<issue-number>-<short-name>), merged into main when done
- Commit messages reference the issue they close
- I used Claude as a pair-programming partner throughout for design discussions, code review, and debugging

**What's next**
-  More features around the shopping list
- Cleaner code structure
- Work with UI
- For more details --> https://github.com/users/andiepvo/projects/1
