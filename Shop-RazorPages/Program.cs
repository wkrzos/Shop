using Microsoft.EntityFrameworkCore;
using Shop_RazorPages.Data;
using Microsoft.AspNetCore.Identity;
using Shop_RazorPages.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireRole("Admin");
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // or db.Database.Migrate();
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Define your desired admin credentials
    string adminEmail = "admin@admin.com";
    string adminPassword = "Admin123!";
    string adminRole = "Admin";

    // (a) Ensure the admin role exists
    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    // (b) Check if there's already an admin user
    var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
    if (existingAdmin == null)
    {
        var adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true // So we don't require email confirmation
        };

        var createResult = await userManager.CreateAsync(adminUser, adminPassword);
        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
        else
        {
            // You may want to log or handle the case where admin creation fails
            // foreach (var error in createResult.Errors) { ... }
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Seed categories if empty
    if (!db.Categories.Any())
    {
        var categories = new List<Category>
        {
            new Category { Name = "Category 1" },
            new Category { Name = "Category 2" }
        };

        db.Categories.AddRange(categories);
        await db.SaveChangesAsync();
    }

    // Seed articles if empty
    if (!db.Articles.Any())
    {
        var categories = db.Categories.ToList(); // existing categories
        var random = new Random();

        // Create 40 articles
        for (int i = 1; i <= 40; i++)
        {
            // Pick a random category
            var randomCategory = categories[random.Next(categories.Count)];

            // Example: random price
            double price = Math.Round(random.NextDouble() * 100, 2);

            var article = new Article
            {
                Name = $"Article {i}",
                Price = (decimal)price,
                CategoryId = randomCategory.Id,
                // Optionally other properties, e.g. ExpiryDate, ImagePath, etc.
            };

            db.Articles.Add(article);
        }

        await db.SaveChangesAsync();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
