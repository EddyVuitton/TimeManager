﻿using TimeManager.Domain.DTOs;
using TimeManager.Domain.Entities;

namespace TimeManager.WebAPI.Repositories.Management;

public interface IManagement
{
    Task<List<ActivityDto>> AddActivityAsync(ActivityDto activity);
    Task<List<HourType>> GetHourTypesAsync();
    Task<List<RepetitionType>> GetRepetitionTypesAsync();
    Task<List<ActivityDto>> GetActivitiesAsync(int userId);
    Task RemoveActivityAsync(int activityId);
    Task<List<ActivityListDto>> GetActivityListsAsync(int userId);
    Task<ActivityDto> UpdateActivityAsync(ActivityDto activity);
    Task<ActivityListDto> AddActivityListAsync(ActivityListDto activityList);
    Task<ActivityListDto> UpdateActivityListAsync(ActivityListDto activityList);
    Task RemoveActivityListAsync(int activityListId);
    Task MoveTaskToListAsync(int taskId, int taskListId);
    Task<List<RepetitionDto>> GetRepetitionsAsync(int userId);
    Task RemoveRepetitionAsync(int repetitionId);
    Task MoveRepetitionToListAsync(int repetitionId, int taskListId);
}