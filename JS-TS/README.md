# FastRouter

A small file-based API router for Fastify.

FastRouter lets you define your API routes in one place while automatically loading the handler files from your routes directory.

## Features

* Built for Fastify
* Automatic route handler loading
* Simple route definitions
* Supports GET, POST, PUT, PATCH, DELETE, HEAD and OPTIONS
* TypeScript support
* No unnecessary framework magic

## Quick Start

Install FastRouter:

```bash
npm install @antalervin19/fastrouter
```

Create a routes directory:

```text
routes/
├── hello.ts
└── users/
    └── me.ts
```

Create a Fastify server:

```ts
import Fastify from "fastify";
import { FastRouter } from "fastrouter";

const server = Fastify();

const router = new FastRouter(server, {
    directory: "./routes"
});

router.get("/hello");
router.get("/users/me");

await router.load();

await server.listen({
    port: 3000
});
```

FastRouter automatically maps:

```text
/hello
```

to:

```text
routes/hello.ts
```

and:

```text
/users/me
```

to:

```text
routes/users/me.ts
```

A route handler looks like this:

```ts
import type { FastifyRequest, FastifyReply } from "fastify";

export default async function (
    request: FastifyRequest,
    reply: FastifyReply
) {
    return reply.send({
        message: "Hello from FastRouter!"
    });
}
```

## Example

A complete working example is included in the repository.

Run it with:

```bash
npm run example
```

Then open:

```text
http://127.0.0.1:3000/hello
```

## Documentation

See [DOCUMENTATION.md](DOCUMENTATION.md) for the full API documentation.

## License

MIT
