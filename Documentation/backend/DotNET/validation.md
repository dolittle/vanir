# Validation

## FluentValidation

For validation, [FluentValidation](https://fluentvalidation.net) is supported.
All types implementing [`AbstractValidator`](https://docs.fluentvalidation.net/en/latest/start.html) will be automatically discovered and
used when appropriate, be it for GraphQL mutations or queries.

> Note:
> For the GraphQL support with [Hot Chocolate](https://chillicream.com/docs/hotchocolate/) there are other ways for doing validation.
> You can read more about them [here](https://chillicream.com/docs/hotchocolate/v10/execution-engine/validation-rules/).
> If you want to hook up other 3rd party validation libraries as well there is an [issue](https://github.com/ChilliCream/hotchocolate/issues/1197)
> describing whats involved.

## Validating an instance of an object

You can validate instances that might have validators associated with it.
The interface `IValidators` found in the namespace `Dolittle.Vanir.Backend.Validation` provides you with
validators and also the capability to evaluate all, if any, validators associated with
a specific type.

```csharp
using Dolittle.Vanir.Backend.Validation;

public class MyThing
{
    IValidators _validators;

    public MyThing(IValidators validators)
    {
        _validators = validators;
    }

    public void DoSomething(MyTypeWithValidators instance)
    {
        var result = _validators.Validate(instance);
        if( !result.IsValid )
        {
            // Handle if not valid; e.g. throw an exception or HTTP error codes
        }
    }
}
```
