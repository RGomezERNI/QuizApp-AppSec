using Microsoft.EntityFrameworkCore;
using QuizAppAPI.Data;
using QuizAppAPI.Profiles.QuizzesMappingProfile;
using QuizAppAPI.Profiles.UsersProfiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<QuizDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddAutoMapper(typeof(UserMappingProfiles));
builder.Services.AddAutoMapper(typeof(QuizzesMappingProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddControllers();
//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
