using System;
using Machine.Specifications;
using PocoDb.Commits;

namespace PocoDb.Specs.Commits
{
    public class when_calling_equals_on_itself : with_a_new_CommitId
    {
        Because of = () => result = sut.Equals(sut);

        It should_be_true = () => result.ShouldBeTrue();

        static bool result;
    }

    public class when_calling_equals_on_a_different_CommitId : with_a_new_CommitId
    {
        Establish c = () => { otherId = new CommitId(Guid.NewGuid(), DateTime.Now); };

        Because of = () => result = sut.Equals(otherId);

        It should_be_true = () => result.ShouldBeFalse();

        static CommitId otherId;
        static bool result;
    }

    public class when_calling_equals_on_a_different_CommitId_with_the_same_id : with_a_new_CommitId
    {
        Establish c = () => { otherId = new CommitId(id, created); };

        Because of = () => result = sut.Equals(otherId);

        It should_be_true = () => result.ShouldBeTrue();

        static CommitId otherId;
        static bool result;
    }

    public class when_comparing_to_itself : with_a_new_CommitId
    {
        Because of = () => result = sut.CompareTo(sut);

        It should_be_true = () => result.ShouldEqual(0);

        static int result;
    }

    public class when_comparing_to_a_newer_CommitId : with_a_new_CommitId
    {
        Establish c = () => { otherId = new CommitId(Guid.NewGuid(), created.AddSeconds(5)); };

        Because of = () => result = sut.CompareTo(otherId);

        It should_be_true = () => result.ShouldBeLessThan(0);

        static CommitId otherId;
        static int result;
    }

    public class when_comparing_to_a_older_CommitId : with_a_new_CommitId
    {
        Establish c = () => { otherId = new CommitId(Guid.NewGuid(), created.AddSeconds(-5)); };

        Because of = () => result = sut.CompareTo(otherId);

        It should_be_true = () => result.ShouldBeGreaterThan(0);

        static CommitId otherId;
        static int result;
    }
}