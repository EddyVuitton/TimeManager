using Microsoft.EntityFrameworkCore;

namespace TimeManager.Domain.Context;

public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
{
}