using System;
using Dolittle.Vanir.Backend.GraphQL;
using FluentValidation;
using MongoDB.Driver;

namespace Backend
{
    public class MyObject
    {
        public string Something { get; set; }
    }

    public class MyObjectValidator : AbstractValidator<MyObject>
    {
        public MyObjectValidator()
        {
            RuleFor(_ => _.Something).NotNull().NotEmpty().WithMessage("This should be set");
        }
    }


    public class Things : GraphController
    {
        private readonly IMongoDatabase _mongoDatabase;

        public Things(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        [Mutation]
        public bool DoSomething(OwnerId input)
        {
            Console.WriteLine($"Hello world - {input}");
            return true;
        }

        [Mutation]
        public bool DoThings(MyObject input)
        {
            Console.WriteLine($"Hello world - {input.Something}");
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
