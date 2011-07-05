using System;
using Machine.Specifications;
using PocoDb.Commits;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Commits
{
    [Subject(typeof (CommitId))]
    public class with_a_new_CommitId : Observes<CommitId>
    {
        Establish c = () => {
            id = Guid.NewGuid();
            created = DateTime.Now;
            sut_factory.create_using(() => new CommitId(id, created));
        };

        protected static Guid id;
        protected static DateTime created;
    }
}