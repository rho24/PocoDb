CREATE TABLE [SqlCommits] (
  [Id] nvarchar(128) NOT NULL
, [Value] nvarchar(1048576) NOT NULL
);
GO
ALTER TABLE [SqlCommits] ADD CONSTRAINT [PK__SqlCommits__Id] PRIMARY KEY ([Id]);
GO
CREATE UNIQUE INDEX [UQ__SqlCommits__Id] ON [SqlCommits] ([Id] ASC);
GO
