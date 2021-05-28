using System;
using System.Threading.Tasks;
using Dolittle.SDK.Aggregates;
using Dolittle.SDK.Concepts;
using Dolittle.SDK.Events;
using Dolittle.SDK.Events.Handling;
using Dolittle.SDK.Events.Store;
using Dolittle.Vanir.Backend.Execution;
using Dolittle.Vanir.Backend.Features;
using Dolittle.Vanir.Backend.GraphQL;
using FluentValidation;
using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Backend
{
    [EventType("5ff11df2-68e6-4890-8cae-b22fd1e3c8b5")]
    public class MyEvent
    {
        public int Something { get; set; }
    }

    [EventHandler("a5023ef8-0226-49c7-bfa2-256c4f3aa6e0", inScope:"8dba121d-b589-8344-9c0c-eb60f5848c98")]
    public class MyEventHandler2
    {

        public Task Handle(MyEvent @event, EventContext context)
        {
            Console.WriteLine("HEllo world");
            return Task.CompletedTask;
        }
    }


    /*
    public class CommonObjectScalar : ScalarType
    {
        public CommonObjectScalar() : base("CommonObject")
        {

        }

        public override Type RuntimeType => typeof(CommonObject);

        public override bool IsInstanceOfType(IValueNode valueSyntax)
        {
            throw new NotImplementedException();
        }

        public override object ParseLiteral(IValueNode valueSyntax, bool withDefaults = true)
        {
            throw new NotImplementedException();
        }

        public override IValueNode ParseResult(object resultValue)
        {
            throw new NotImplementedException();
        }

        public override IValueNode ParseValue(object runtimeValue)
        {
            throw new NotImplementedException();
        }

        public override bool TryDeserialize(object resultValue, out object runtimeValue)
        {
            throw new NotImplementedException();
        }

        public override bool TrySerialize(object runtimeValue, out object resultValue)
        {
            throw new NotImplementedException();
        }
    }
    */

    public class CommonObject
    {
        public string Something { get; set; }
    }

    public class SomeConcept : ConceptAs<string>
    {
    }

    public class NestedObject
    {
        public SomeConcept DeepConcept { get; set; }
    }

    public class SomeConceptValidator : AbstractValidator<SomeConcept>
    {
        public SomeConceptValidator()
        {
            RuleFor(_ => _.Value).NotNull().NotEmpty().WithMessage("The string must have content");
        }
    }

    public class UserId : ConceptAs<Guid>
    {
    }

    public class MyObject
    {
        public string Something { get; set; }
        public int SomeNumber { get; set; }
        public SomeConcept Concept { get; set; }
        public NestedObject Nested { get; set; }
        public UserId UserId { get; set; }

        //[GraphQLType(typeof(CommonObjectScalar))]
        public CommonObject CommonObject { get; set; }
    }

    public class MyObjectValidator : AbstractValidator<MyObject>
    {
        public MyObjectValidator()
        {
            RuleFor(_ => _.Something).NotNull().NotEmpty().WithMessage("This should be set");
        }
    }

    public class MySecondObjectValidator : AbstractValidator<MyObject>
    {
        public MySecondObjectValidator()
        {
            RuleFor(_ => _.SomeNumber).GreaterThan(42).WithMessage("Must be greater than 42");
            RuleFor(_ => _.Concept).SetValidator(new SomeConceptValidator());
        }
    }

    [AggregateRoot("01ad9a9f-711f-47a8-8549-43320f782a1e")]
    public class Kitchen : AggregateRoot
    {
        int _counter;

        public Kitchen(EventSourceId eventSource)
            : base(eventSource)
        {
        }

        public void PrepareDish(string dish, string chef)
        {
            if (_counter >= 2) throw new Exception("Cannot prepare more than 2 dishes");
            Apply(new DishPrepared { Dish = dish, Chef = chef });
            ApplyPublic(new DishPrepared { Dish = dish, Chef = chef });
            Console.WriteLine($"Kitchen Aggregate {EventSourceId} has applied {_counter} {typeof(DishPrepared)} events");
        }

        void On(DishPrepared @event) => _counter++;
    }

    [EventType("2977fd82-9614-4082-ab8e-d436ed129248")]
    public class DishPrepared
    {
        public string Dish { get; init; }
        public string Chef { get; init; }

        public CommonObject CommonObject { get; }

        public DishPrepared()
        {
            CommonObject = new CommonObject();
        }
    }

    [EventHandler("db2bb639-937c-49ca-946e-ad3868882080")]
    public class MyEventHandler
    {
        readonly ILogger<MyEventHandler> _logger;

        public MyEventHandler(ILogger<MyEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(DishPrepared @event, EventContext context)
        {
            _logger.LogInformation($"{@event.Chef} has prepared {@event.Dish}. Yummm!");
            await Task.CompletedTask;
        }
    }


    public class Things : GraphController
    {
        readonly IMongoDatabase _mongoDatabase;
        readonly IEventStore _eventStore;
        readonly IAggregateOf<Kitchen> _kitchen;

        public Things(IMongoDatabase mongoDatabase, IEventStore eventStore, IAggregateOf<Kitchen> kitchen, IExecutionContextManager executionContextManager)
        {
            _mongoDatabase = mongoDatabase;
            _eventStore = eventStore;
            _kitchen = kitchen;
        }

        [Mutation]
        public async Task<bool> DoMore()
        {
            //_eventStore.CommitEvent(new DishPrepared("Bean Blaster Taco", "Mr. Taco"), Guid.NewGuid());
            await _kitchen
                .Get(Guid.NewGuid())
                .Perform(_ => _.PrepareDish("Bean Blaster Taco", "Mr. Taco"));
            return true;
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
        [Feature("my.first.feature")]
        public Owner TheOwner(int id) => new Owner
        {
            Id = Guid.NewGuid(),
            Name = "Blah"
        };

        [Query]
        public Owner Something()
        {
            return new Owner();
        }
    }
}
