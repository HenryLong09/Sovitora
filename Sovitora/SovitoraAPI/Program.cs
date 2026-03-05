var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Add controllers and JSON support
builder.Services.AddControllers().AddNewtonsoftJson();

// 2️⃣ Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 3️⃣ Enable CORS
app.UseCors("AllowAll");

// 4️⃣ Map controllers
app.MapControllers();

// 5️⃣ Run the API
app.Run();