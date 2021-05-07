# Validation

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
