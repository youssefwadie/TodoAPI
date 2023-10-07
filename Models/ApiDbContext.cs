using Microsoft.EntityFrameworkCore;

namespace TodoAPI.Models;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions options) : base(options) 
    {
        
    }

    public DbSet<TodoItem> TodoItems { get; set; }

}