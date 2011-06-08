using System;
using System.Collections.Generic;
using Machine.Specifications;
using PocoDb.Meta;

namespace PocoDb.Specs.Indexing
{
    public class when_GetIds_is_called : with_a_populated_TypeIndex
    {
        Because of = () => ids = sut.GetIds();

        It should_return_its_ids = () => ids.ShouldContainOnly(id);

        static IEnumerable<IPocoId> ids;
    }
}