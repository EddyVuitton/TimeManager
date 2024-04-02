create procedure p_get_user_activities @userId int
as
begin
	select
		a.ID as ActivityId,
		r.ID as RepetitionId,
		a.ActivityListId,
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

create procedure p_remove_user_activity_list @id int
as
begin
	declare @is_default int = (
		select top 1 IsDefault
		from ActivityList
		where Id = @id
	);

	if (@is_default = 1)
	begin
		raiserror('Listy domyslnej nie mozna usunac', 16, 1);
		return;
	end

	delete r
	from Repetition r
	join Activity a on r.Id = a.RepetitionId
	where a.ActivityListId = @id;

	delete a
	from Activity a
	where a.ActivityListId = @id;

	delete ActivityList where Id = @id;
end
go