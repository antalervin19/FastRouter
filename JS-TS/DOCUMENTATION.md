# FastRouter Documentation

## Overview

FastRouter is a lightweight routing layer for Fastify.

It separates route definitions from route implementations.

Instead of putting the implementation directly inside your server:

```ts
server.get("/users/me", async (request, reply) => {
    // ...
});
```

you define the route:

```ts
router.get("/users/me");
```

and FastRouter loads:

```text
routes/users/me.ts
```

automatically.

---

# Installation

```bash
npm install @antalervin19/fastrouter
```

FastRouter requires Fastify as a peer dependency.

```bash
npm install fastify
```

---

# Basic Setup

```ts
import Fastify from "fastify";
import { FastRouter } from "fastrouter";

const server = Fastify({
    logger: true
});

const router = new FastRouter(server, {
    directory: "./routes"
});

router.get("/hello");
router.get("/users/me");

await router.load();

await server.listen({
    port: 3000,
    host: "127.0.0.1"
});
```

The `directory` option specifies where FastRouter looks for route handlers.

---

# Route Files

Routes mirror the URL structure.

Given:

```text
routes/
├── hello.ts
├── users/
│   ├── me.ts
│   └── profile.ts
└── auth/
    ├── login.ts
    └── logout.ts
```

You can define:

```ts
router.get("/hello");
router.get("/users/me");
router.get("/users/profile");

router.post("/auth/login");
router.post("/auth/logout");
```

FastRouter resolves them to:

```text
/hello               → routes/hello.ts
/users/me            → routes/users/me.ts
/users/profile       → routes/users/profile.ts
/auth/login          → routes/auth/login.ts
/auth/logout         → routes/auth/logout.ts
```

---

# Route Handlers

Every route file must export a default function.

```ts
import type { FastifyRequest, FastifyReply } from "fastify";

export default async function (
    request: FastifyRequest,
    reply: FastifyReply
) {
    return reply.send({
        message: "Hello!"
    });
}
```

The handler receives the normal Fastify request and reply objects.

This means existing Fastify functionality can be used normally.

For example:

```ts
export default async function (
    request: FastifyRequest,
    reply: FastifyReply
) {
    return reply.code(201).send({
        created: true
    });
}
```

---

# HTTP Methods

FastRouter supports:

```ts
router.get("/example");
router.post("/example");
router.put("/example");
router.patch("/example");
router.delete("/example");
router.head("/example");
router.options("/example");
```

You can also use the generic `route()` method:

```ts
router.route("GET", "/example");
```

---

# Custom Handler Paths

The automatic path mapping can be overridden.

```ts
router.get("/auth/google", "auth/googleLogin");
```

This loads:

```text
routes/auth/googleLogin.ts
```

This is useful when the URL structure doesn't match the desired file structure.

---

# Route Parameters

FastRouter passes route definitions directly to Fastify, so Fastify route parameters can be used:

```ts
router.get("/users/:id");
```

Handler:

```ts
import type { FastifyRequest, FastifyReply } from "fastify";

export default async function (
    request: FastifyRequest,
    reply: FastifyReply
) {
    const { id } = request.params as {
        id: string;
    };

    return reply.send({
        userId: id
    });
}
```

A request to:

```text
/users/123
```

will provide:

```text
id = 123
```

---

# Route Loading

Routes are registered when:

```ts
await router.load();
```

is called.

FastRouter loads each declared handler and registers it with the Fastify instance.

The filesystem is used to locate handlers, but routes are still explicitly declared in the server.

For example:

```ts
router.get("/users/me");
```

is what creates the route.

Simply creating:

```text
routes/users/me.ts
```

does not automatically expose an endpoint.

---

# Project Structure

A typical FastRouter project looks like:

```text
my-api/
├── src/
│   └── index.ts
│
├── routes/
│   ├── auth/
│   │   ├── login.ts
│   │   └── logout.ts
│   │
│   └── users/
│       ├── me.ts
│       └── profile.ts
│
├── package.json
└── tsconfig.json
```

The server remains responsible for defining the public API:

```ts
router.get("/auth/login");
router.post("/auth/logout");

router.get("/users/me");
router.get("/users/profile");
```

The route files contain the implementation.

---

# Example

FastRouter includes a small example project.

Run:

```bash
npm run example
```

The example starts a Fastify server on:

```text
http://127.0.0.1:3000
```

Available endpoints:

```text
GET /hello
GET /users/me
```

---

# API

## `new FastRouter(server, options)`

Creates a FastRouter instance.

```ts
const router = new FastRouter(server, {
    directory: "./routes"
});
```

### Options

| Option      | Type     | Description                         |
| ----------- | -------- | ----------------------------------- |
| `directory` | `string` | Directory containing route handlers |

---

## `router.get(path, handler?)`

Registers a GET route.

```ts
router.get("/users/me");
```

Optional custom handler:

```ts
router.get("/users/me", "users/currentUser");
```

---

## `router.post(path, handler?)`

Registers a POST route.

```ts
router.post("/users");
```

---

## `router.put(path, handler?)`

Registers a PUT route.

```ts
router.put("/users/:id");
```

---

## `router.patch(path, handler?)`

Registers a PATCH route.

```ts
router.patch("/users/:id");
```

---

## `router.delete(path, handler?)`

Registers a DELETE route.

```ts
router.delete("/users/:id");
```

---

## `router.head(path, handler?)`

Registers a HEAD route.

```ts
router.head("/health");
```

---

## `router.options(path, handler?)`

Registers an OPTIONS route.

```ts
router.options("/users");
```

---

## `router.route(method, path, handler?)`

Registers a route using an explicit HTTP method.

```ts
router.route("GET", "/health");
```

---

## `router.load()`

Loads all declared route handlers and registers them with Fastify.

```ts
await router.load();
```

This should be called after all routes have been declared and before starting the Fastify server.

---

# Design Philosophy

FastRouter intentionally stays small.

It does not try to replace Fastify.

Fastify handles:

* HTTP
* requests
* responses
* plugins
* hooks
* validation
* middleware
* servers

FastRouter handles one thing:

**connecting clean route definitions to organized handler files.**

The goal is to keep API projects easy to navigate without introducing a large framework.
