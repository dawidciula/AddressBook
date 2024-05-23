using Microsoft.EntityFrameworkCore;
using Address_Book.Data;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddDbContext<AddressBookDbContext>(options =>
    options.UseInMemoryDatabase("AddressBookEntriesDatabase"));

//Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Minimalny poziom logowania
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Ustawienie poziomu logowania dla kategorii Microsoft na Warning
    .WriteTo.Console() // Logowanie do konsoli
    .CreateLogger();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

//Add serilog to host
builder.Host.UseSerilog();

var app = builder.Build();


//Database inicialization
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AddressBookDbContext>();
    AddressBookDbContextInitializer.Initialize(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//Add serilog middleware for logging HTTP requests
app.Use(async (context, next) =>
{
    Log.Information("Incoming Request:");
    Log.Information("Method: {RequestMethod}", context.Request.Method);
    Log.Information("Path: {RequestPath}", context.Request.Path);

    Log.Information("Headers:");
    foreach (var (key, value) in context.Request.Headers)
    {
        Log.Information("{HeaderKey}: {HeaderValue}", key, value);
    }

    Log.Information("Request Body:");
    context.Request.EnableBuffering();
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    context.Request.Body.Position = 0;
    Log.Information("{RequestBody}", requestBody);
    
    var originalBodyStream = context.Response.Body;
    using (var responseBody = new MemoryStream())
    {
        context.Response.Body = responseBody;

        await next();

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBodyContent = await new StreamReader(context.Response.Body).ReadToEndAsync();
        Log.Information("Outgoing Response:");
        Log.Information("Status Code: {StatusCode}", context.Response.StatusCode);
        Log.Information("Response Body:");
        Log.Information("{ResponseBody}", responseBodyContent);

        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);
    }
});



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();