using chatapp.Data;
using chatapp.DataAccess;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ChatAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatAppDatabase")));
builder.Services.AddScoped<ConnectDB>();
builder.Services.AddScoped<Token>();
builder.Services.AddScoped<Dangky>();
builder.Services.AddScoped<DangNhap>();
builder.Services.AddScoped<Inforuser>();
builder.Services.AddScoped<FindUser>();
builder.Services.AddScoped<TDDangNhap>();
builder.Services.AddScoped<AddUser>();
builder.Services.AddScoped<Channelname>();
builder.Services.AddScoped<DKChannel>();
builder.Services.AddScoped<DKGroup>();
builder.Services.AddScoped<Groupname>();
builder.Services.AddScoped<ReceiveMess>();
builder.Services.AddScoped<SendMess>();
builder.Services.AddScoped<savefileinfo>();
builder.Services.AddScoped<Friend>();
builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
