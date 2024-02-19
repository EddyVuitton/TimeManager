create procedure p_get_user_activities @userId int
as
begin
	select
		a.Day,
		a.Title,
		a.Description,
		a.Task,
		a.HourTypeId,
		rt.Id [RepetitionTypeId],
		cast(0 as bit) as IsOpen,
		a.UserId
	from Activity a
	join Repetition r on a.RepetitionId = r.Id
	join RepetitionType rt on r.RepetitionTypeId = rt.Id
	where a.UserId = @userId
end
go