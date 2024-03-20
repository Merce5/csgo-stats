using infrastructure.Clients;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Setup DB
builder.Services.AddDbContext<CsgoDbContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultValue"))
);


var app = builder.Build();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
