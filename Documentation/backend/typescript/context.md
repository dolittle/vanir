# Context

As part of every web request coming into the system, there is an object that is set up that
can be retrieved anywhere as part of the request. This holds the following:

```typescript
{
    userId: string;
    tenantId: TenantId;
    cookies: string;
}
```

To access it, all you need to do is use the `getCurrentContext()` function from the context.

```typescript
import { getCurrentContext } from '@dolittle/vanir-backend/dist/web';


const context = getCurrentContext();
```
