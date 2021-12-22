using Microsoft.EntityFrameworkCore;
using BookAPI.Data;
using BookAPI.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<BookDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));
var app = builder.Build();

app.MapGet("/", () =>
" ---------------------------------------------------------------------------------- \n" +
"|------------------------------------ BookAPI -------------------------------------|\n" +
"|        URL                            ||                Response                 |\n" +
"|----------------------------------------------------------------------------------|\n" +
"| => /_api/getbooks                     ||  Get all books.                         |\n" +
"| => /_api/getbooksbyid/{id}            ||  Get book with the specified id.        |\n" +
"| => /_api/getbooksbyname/{name}        ||  Get book with the specified name.      |\n" +
"| => /_api/getbooksbyauthor/{author}    ||  Get all books by the specified author. |\n" +
" ----------------------------------------------------------------------------------");

app.MapGet("/_api/getBooks", async (BookDbContext _db) =>
{
    var booksFromDb = await _db.Books.ToListAsync();
    return Results.Ok(booksFromDb);
});

app.MapGet("/_api/getbooksbyid/{id}", async (int id, BookDbContext _db) =>
{
    return await _db.Books.FindAsync(id)
            is Book book ? Results.Ok(book) : Results.NotFound();
});

app.MapGet("/_api/getbooksbyname/{name}", async(BookDbContext _db, string name) =>
{
    return await _db.Books.Where(b => b.Name.Contains(name)).ToListAsync();
});

app.MapGet("/_api/getbooksbyauthor/{author}", async (BookDbContext _db, string author) =>
{
    return await _db.Books.Where(b => b.Author.Contains(author)).ToListAsync();
});

app.MapPost("/_api/books", async (Book book, BookDbContext _db) =>
{
    _db.Books.Add(book);
    await _db.SaveChangesAsync();

    return Results.Created($"/_api/books/{book.ID}", book);
});

app.Run();