﻿using Microsoft.EntityFrameworkCore;
using TimeManager.Domain.Entities;
using TimeManager.Domain.Extensions;

namespace TimeManager.Domain.Context;

public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
{
    public DbSet<RepetitionType> RepetitionType => Set<RepetitionType>();
    public DbSet<Repetition> Repetition => Set<Repetition>();
    public DbSet<Activity> Activity => Set<Activity>();
    public DbSet<UserAccount> UserAccount => Set<UserAccount>();
    public DbSet<HourType> HourType => Set<HourType>();
    public DbSet<ActivityList> ActivityList => Set<ActivityList>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddDtos();
    }
}