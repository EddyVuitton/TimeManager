using Microsoft.EntityFrameworkCore;
using TimeManager.Domain.Entities;

namespace TimeManager.Domain.Context;

public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
{
    public DbSet<RepetitionType> RepetitionType { get; set; }
    public DbSet<Repetition> Repetition { get; set; }
    public DbSet<Activity> Activity { get; set; }
    public DbSet<User> User { get; set; }
}