using Microsoft.EntityFrameworkCore;
using TimeManager.Domain.DTOs;

namespace TimeManager.Domain.Extensions;

public static class ModelBuilderExtension
{
    public static void AddDtos(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityDto>(entity =>
        {
            entity.HasNoKey().ToView(null);
        });
    }
}