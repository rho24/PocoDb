CREATE TABLE [SqlCommits] (
  [Id] nvarchar(MAX) NOT NULL
, [Value] nvarchar(MAX) NOT NULL
, [SequenceNumber] int NOT NULL IDENTITY (1,1)
);
GO
ALTER TABLE [SqlCommits] ADD CONSTRAINT [PK__SqlCommits__Id] PRIMARY KEY ([Id]);
GO
CREATE UNIQUE INDEX [UQ__SqlCommits__Id] ON [SqlCommits] ([Id] ASC);
GO
CREATE UNIQUE INDEX [UQ__SqlCommits__SequenceNumber] ON [SqlCommits] ([SequenceNumber] ASC);
GO
