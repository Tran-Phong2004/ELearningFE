var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache(); // Bộ nhớ tạm cho Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Thời gian hết hạn Session
    options.Cookie.HttpOnly = true; // Chỉ truy cập qua HTTP
    options.Cookie.IsEssential = true; // Cookie cần thiết để hoạt động
    options.Cookie.SameSite = SameSiteMode.Lax; // Cấu hình SameSite
    options.Cookie.Name = ".ELearningSession"; // Đặt tên cookie khác nếu cần
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSession();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
