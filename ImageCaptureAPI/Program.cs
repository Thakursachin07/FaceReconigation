using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Set Kestrel to listen on port 5229
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5229);
});

// Add services
builder.Services.AddControllers(); // For ImageUploadController
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Enable CORS
app.UseCors();

// Use controllers
app.MapControllers();

app.Run();
// Optional: Minimal API endpoint (e.g., /api/upload)
// app.MapPost("/api/upload", async (HttpRequest request) =>
// {
//     if (!request.HasFormContentType)
//     {
//         return Results.BadRequest("Expected multipart/form-data");
//     }

//     var form = await request.ReadFormAsync();
//     var file = form.Files["image"];

//     if (file == null || file.Length == 0)
//     {
//         return Results.BadRequest("No file uploaded");
//     }

//     using var memoryStream = new MemoryStream();
//     await file.CopyToAsync(memoryStream);
//     var imageBytes = memoryStream.ToArray();

//     Console.WriteLine($"Received file: {file.FileName}");
//     Console.WriteLine($"Size: {file.Length} bytes");
//     Console.WriteLine(imageBytes);

//     return Results.Ok(new { message = "Image received in memory" });
// });


