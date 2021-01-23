using System;
using System.ComponentModel;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using GraphQL.AspNet.Interfaces.Controllers;

namespace Backend
{
    public class Things : GraphController
    {
        [QueryRoot]
        public Owner TheOwner(int id)
        {
            return new Owner
            {
                Id = Guid.NewGuid(),
                Name = "Blah"
            };
        }

        [Query]
        [Description("This thing is awesome")]
        public Owner Something()
        {
            return new Owner();
        }
    }
}
