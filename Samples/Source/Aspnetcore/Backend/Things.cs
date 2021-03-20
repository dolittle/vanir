using System;
using System.ComponentModel;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using MongoDB.Driver;

namespace Backend
{
    public class Things : GraphController
    {
        private readonly IMongoDatabase _mongoDatabase;

        public Things(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

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
