# Event Serialization

Dolittle is all about [events](https://dolittle.io/docs/concepts/events/). The events are stored in our
[event store](https://dolittle.io/docs/concepts/event_store/) and for events that one wants to communicate between
microservices we call [public](https://dolittle.io/docs/concepts/events/#public-vs-private) events and are
going through our [event horizon](https://dolittle.io/docs/concepts/event_horizon/).

Common to all of this is that we communicate using the [Dolittle SDK](https://github.com/dolittle/DotNET.SDK).
The SDK leverages the defacto standard serializer called [Newtonsoft (Json.NET)](https://www.newtonsoft.com/json).
With this in mind, you can pretty much rely on any of its capabilities to achieve what you want with the
events going and coming out of the Dolittle Runtime.

## Casing

The event is saved as is - meaning that if you use [UpperCamelCasing/PascalCase](https://en.wikipedia.org/wiki/Camel_case), you will see the properties in the event log as
that. However, the Newtonsoft serializer will be able to deserialize from lowerCamelCase to an UpperCamelCase.

## Excluding properties one does not need

If you are consuming events from another microservice, but aren't interested in all the properties on the event coming
from the producer, the serializer is not strict and will ignore these extra properties. You can therefor safely have
more properties on the producer and then model the event as you see fit in the consumer.

For instance, if we have the following event coming from a producer:

```csharp
[EventType("ef33b96d-271f-429f-a9cd-cd7fad1db8ae")]
public class EmployeeHired
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime StartDate { get; init; }

}
```

If you're not interested in the start date in a consumer you could define this event to be:

```csharp
[EventType("ef33b96d-271f-429f-a9cd-cd7fad1db8ae")]
public class EmployeeHired
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
}
```

## Adding properties

Over time you might find that you need to add properties as you want to capture more information on the event.
Since the serializer is not strict and there is no strict schema on how the events look like in the event store,
it will allow you to do so. Since there will be events that has been written to the event store that does not carry
any new properties; it is recommended to set a default value for these.

Lets say you have the following event:

```csharp
[EventType("ef33b96d-271f-429f-a9cd-cd7fad1db8ae")]
public class EmployeeHired
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime StartDate { get; init; }

}
```

And you get a requirement change where you want to capture at one point which department the person will be working in from day one,
you can simply do the following:

```csharp
[EventType("ef33b96d-271f-429f-a9cd-cd7fad1db8ae")]
public class EmployeeHired
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime StartDate { get; init; }
    public string Department { get; init; } = "Unspecified";    // Default value for existing events without this property
}
```

If you want to remove properties over time, that is also perfectly valid. These will just be ignored in the serializer.

## Renaming properties

If the producer has a different domain language than the consumer, or even a different language preference for naming types and properties
you want to be able specify the names of these.

The Newtonsoft serializer supports an attribute called [[JsonProperty]](https://www.newtonsoft.com/json/help/html/JsonPropertyName.htm).
This attribute can then be used to rename properties. The type name can by design be anything, because the Dolittle Runtime works with the
`Guid` specified in the `[EventType]` attribute.

So if we take the following event:

```csharp
[EventType("ef33b96d-271f-429f-a9cd-cd7fad1db8ae")]
public class EmployeeHired
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime StartDate { get; init; }
    public string Department { get; init; }
}
```

We can change the language of this in another microservice completely, for instance into Norwegian:

```csharp
using Newtonsoft.Json;

[EventType("ef33b96d-271f-429f-a9cd-cd7fad1db8ae")]
public class PersonAnsatt
{
    [JsonProperty("FirstName")]
    public string Fornavn { get; init; }

    [JsonProperty("LastName")]
    public string Etternavn { get; init; }

    [JsonProperty("StartDate")]
    public DateTime StartDato { get; init; }

    [JsonProperty("Department")]
    public string Avdeling { get; init; }
}
```
