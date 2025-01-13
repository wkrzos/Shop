using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;


public static class DbSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" },
                new Category { Id = 3, Name = "Clothing" }
            );
            context.SaveChanges();
        }

        if (!context.Articles.Any())
        {
            context.Articles.AddRange(
                new Article
                {
                    Name = "Laptop",
                    Price = 1200.99m,
                    ImagePath = "/uploaded/laptop.jpg",
                    ExpiryDate = DateTime.UtcNow.AddYears(2),
                    CategoryId = 1
                },
                new Article
                {
                    Name = "Smartphone",
                    Price = 699.99m,
                    ImagePath = "/uploaded/smartphone.jpg",
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    CategoryId = 1
                },
                new Article
                {
                    Name = "Novel",
                    Price = 19.99m,
                    ImagePath = "/uploaded/novel.jpg",
                    ExpiryDate = DateTime.UtcNow.AddYears(5),
                    CategoryId = 2
                }
            );
        }

        context.SaveChanges();
    }
}
