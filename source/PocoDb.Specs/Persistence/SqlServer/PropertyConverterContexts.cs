using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using Newtonsoft.Json;
using PocoDb.Persistence.SqlServer;

namespace PocoDb.Specs.Persistence.SqlServer
{
    [Subject(typeof (PropertyConverter))]
    public class with_a_new_PropertyConverter : Observes<PropertyConverter>
    {
        Establish c = () => {
            settings = new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.Objects};
            sut_setup.run(sut => settings.Converters.Add(sut));
        };

        protected static JsonSerializerSettings settings;
    }
}