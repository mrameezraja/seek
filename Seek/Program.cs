using Seek.Contracts;
using Seek.Database;
using Seek.Services;
using Serilog;
using Slugify;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(Path.GetFullPath("Logs/Log_.txt"), rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 10485760, rollOnFileSizeLimit: true, retainedFileCountLimit: 10)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog();

builder.Services.AddEntityFrameworkSqlite().AddDbContext<SeekDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IDatabaseService, DatabaseService>();
builder.Services.AddTransient<ISlugHelper>(_ => new SlugHelper(new SlugHelperConfiguration
{
    StringReplacements = new()
    {
        [" "] = "_"
    }
}));

builder.Services.AddMemoryCache();

builder.Services.Configure<List<Course>>(builder.Configuration.GetSection("Seek:Courses"));

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// app.Services.GetService<IDatabaseService>()?.Init();

app.Run();
