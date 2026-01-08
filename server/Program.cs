using InboxEngine.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO: Register the PriorityScoringService using Dependency Injection
// Use AddSingleton to register IPriorityScoringService with PriorityScoringService implementation
// to ensure that the service for Priority Scoring lives for the duration of the app (since we need to 
// set Today's Date)
builder.Services.AddSingleton<IPriorityScoringService, PriorityScoringService>();

// TODO: Configure CORS to allow requests from your frontend
// Common frontend URLs: http://localhost:3000, http://127.0.0.1:5500, http://localhost:5500
// Make sure to allow any header and any method
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:3000",
                                  "http://127.0.0.1:5500",
                                  "http://localhost:5500")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO: Enable CORS middleware here (must be before other middleware like UseAuthorization)
app.UseCors(MyAllowSpecificOrigins);

// Only use HTTPS redirection in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
