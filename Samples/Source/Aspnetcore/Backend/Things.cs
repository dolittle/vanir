using System;
using Dolittle.Vanir.Backend.GraphQL;
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

        [Mutation]
        public bool DoSomething()
        {
            Console.WriteLine("Hello world");
            return true;
        }


        [Query("MyCustomName")]
        public Owner TheOwner(int id)
        {
            return new Owner
            {
                Id = Guid.NewGuid(),
                Name = "Blah"
            };
        }

        [Query]
        public Owner Something()
        {
            return new Owner();
        }
    }
}
