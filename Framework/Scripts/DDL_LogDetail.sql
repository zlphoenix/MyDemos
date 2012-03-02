create table LogDetails (
id bigint identity not null,
LogDate datetime null,
[Thread] nvarchar(max),
[Level] nvarchar(max),
[Logger]nvarchar(max),
[Message] nvarchar(max)
)