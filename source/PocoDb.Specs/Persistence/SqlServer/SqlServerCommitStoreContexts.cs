using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Persistence.SqlServer;

namespace PocoDb.Specs.Persistence.SqlServer
{
    [Subject(typeof (SqlServerCommitStore))]
    public class with_a_new_SqlServerCommitStore : Observes<SqlServerCommitStore>
    {
        Establish c = () => {
            SQLCEEntityFramework.Start();
            depends.on<string>("PocoDbTestDb");
        };
    }
}