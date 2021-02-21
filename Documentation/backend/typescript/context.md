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

## Express Middleware

The setting of this context is done through an express middleware.
It looks at the HTTP header and sets up the context for the current request coming in.
The default setup gets set up with this, but if you have your own setup and want to leverage
this functionality - you can do so through the following:

```typescript
import express from 'express';
import { ContextMiddleware } from '@dolittle/vanir-backend/dist/web';

const app = express();
app.use(ContextMiddleware);
```
