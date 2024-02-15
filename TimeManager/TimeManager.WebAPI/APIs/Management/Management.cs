using System.Data;
using TimeManager.Domain.Context;
using TimeManager.Domain.DTOs;
using TimeManager.WebAPI.Extensions;

namespace TimeManager.WebAPI.APIs.Management;

public class Management(DBContext context) : IManagementContext
{
    private readonly DBContext _context = context;

    #region PublicMethods

    public async Task<List<ActivityDto>> GetUserActivitiesAsync(int userId)
    {
        var hT = new object[]
        {
            _context.CreateParameter("userId", userId, SqlDbType.Int)
        };
        var result = await _context.SqlQueryAsync<ActivityDto>("exec p_get_user_activities @userId;", hT);

        return result ?? [];
    }

    #endregion PublicMethods
}