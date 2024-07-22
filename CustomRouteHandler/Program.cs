using CustomRouteHandler.Models.Handlers;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment env = builder.Environment;


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();  // wwwroot altinda medya turlerine ulasabilmek icin UseStaticFile ' kullanilmasi zorunludur.

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoint =>
{

	endpoint.Map("image/{fileName}", new ImageHandler().Handle(env.WebRootPath));

});



app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
