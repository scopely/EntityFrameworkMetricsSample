using EntityFrameworkMetricsSample;
using System;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;

using var db = new BloggingContext();
DbInterception.Add(new MetricsInterceptor());

Console.WriteLine("Press any key to stop.");

var watch = Stopwatch.StartNew();

while (!Console.KeyAvailable)
{
    // Create
    db.Blogs.Add(new Blog { Name = "test" });
    db.SaveChanges();

    // Read
    var blog = db.Blogs
        .OrderBy(b => b.BlogId)
        .First();

    // Update
    blog.Name = "test2";
    (blog.Posts ??= new List<Post>()).Add(
        new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
    db.SaveChanges();

    // Delete
    db.Blogs.Remove(blog);
    db.SaveChanges();

    if (watch.Elapsed > TimeSpan.FromSeconds(2))
    {
        watch.Restart();
        MetricsReporter.Dump();
    }
}