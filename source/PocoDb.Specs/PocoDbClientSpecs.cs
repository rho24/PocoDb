using System;
using System.Linq;
using Machine.Specifications;
using PocoDb.Session;

namespace PocoDb.Specs
{
    public class when_BeginSession_is_called : with_a_new_PocoDbClient
    {
        Because of = () => session = sut.BeginSession();

        It should_return_a_PocoSession = () => session.ShouldBeOfType<PocoSession>();

        static IPocoSession session;
    }

    public class when_BeginWritableSession_is_called : with_a_new_PocoDbClient
    {
        Because of = () => session = sut.BeginWritableSession();

        It should_return_a_PocoSession = () => session.ShouldBeOfType<WritablePocoSession>();

        It should_have_no_changes = () => session.Changes.AllChanges.Count().ShouldEqual(0);

        static IWritablePocoSession session;
    }

    public class when_BeginSession_is_called_before_initialisation
    {
        It should_throw;
    }

    public class when_BeginWritableSession_is_called_before_initialisation
    {
        It should_throw;
    }
}