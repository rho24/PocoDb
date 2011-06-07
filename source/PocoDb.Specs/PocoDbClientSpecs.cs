using System;
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

        static IPocoSession session;
    }
}