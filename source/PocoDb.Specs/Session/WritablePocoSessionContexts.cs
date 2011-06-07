﻿using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Server;
using PocoDb.Session;

namespace PocoDb.Specs.Session
{
    [Subject(typeof (IWritablePocoSession))]
    public class with_a_new_WritablePocoSession : Observes<WritablePocoSession>
    {
        Establish c = () => { pocoDbServer = depends.on<IPocoDbServer>(); };

        protected static IPocoDbServer pocoDbServer;
    }
}