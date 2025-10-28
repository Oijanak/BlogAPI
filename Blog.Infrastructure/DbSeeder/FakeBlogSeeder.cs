
    using BlogApi.Application.Interfaces;
    using BlogApi.Domain.Enum;
    using BlogApi.Domain.Models;
    using Bogus;
    using Microsoft.EntityFrameworkCore;

    public static class FakeBlogSeeder
    {
        public static async Task SeedAsync(IBlogDbContext context)
        {

            if (await context.Blogs.AnyAsync())
            {
                return;
            }
            var authors = await context.Authors.ToListAsync();
            var users=await context.Users.ToListAsync();
        
            if (!authors.Any())
            {
                Console.WriteLine("⚠️ No authors found. Seed authors first before blogs.");
                return;
            }
           
            if (!users.Any())
            {
                Console.WriteLine("⚠️ No users found. Seed users first before blogs.");
                return;
            }
            var blogFaker = new Faker<Blog>()
                .RuleFor(b => b.BlogId, f => Guid.NewGuid())
                .RuleFor(b => b.BlogTitle, f => f.Lorem.Sentence(6))
                .RuleFor(b => b.BlogContent, f => f.Lorem.Paragraphs(3))
                .RuleFor(b => b.StartDate, f => f.Date.Past(2))
                .RuleFor(b => b.EndDate, (f, b) => b.StartDate.AddMonths(f.Random.Int(1, 6)))
                .RuleFor(b => b.CreatedBy, f => f.PickRandom(users).Id)
                .RuleFor(b => b.CreatedAt, f => f.Date.Past(1))
                .RuleFor(b => b.UpdatedAt, f => f.Date.Recent(30))
                .RuleFor(b => b.ActiveStatus, f => f.PickRandom<ActiveStatus>())
                .RuleFor(b => b.ApproveStatus, f => f.PickRandom<ApproveStatus>())
                .RuleFor(b => b.AuthorId, f => f.PickRandom(authors).AuthorId);


        var fakeBlogs = blogFaker.Generate(10000);
        await context.Blogs.AddRangeAsync(fakeBlogs);
        await context.SaveChangesAsync();
        Console.WriteLine("Successfully seeded 10,000 fake blogs!");
        }
    }
