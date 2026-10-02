import type { FastifyInstance } from "fastify";
import { RouteLoader } from "./RouteLoader.js";
import type { HttpMethod, RouteDefinition } from "./types.js";

export class FastRouter {
    private readonly routes: RouteDefinition[] = [];
    private readonly loader: RouteLoader;

    constructor(
        private readonly server: FastifyInstance,
        options: {
            directory: string;
        }
    ) {
        this.loader = new RouteLoader(options.directory);
    }

    route(
        method: HttpMethod,
        path: string,
        handler?: string
    ) {
        this.routes.push({
            method,
            path,
            handler: handler ?? this.pathToHandler(path)
        });

        return this;
    }

    get(path: string, handler?: string) {
        return this.route("GET", path, handler);
    }

    post(path: string, handler?: string) {
        return this.route("POST", path, handler);
    }

    put(path: string, handler?: string) {
        return this.route("PUT", path, handler);
    }

    patch(path: string, handler?: string) {
        return this.route("PATCH", path, handler);
    }

    delete(path: string, handler?: string) {
        return this.route("DELETE", path, handler);
    }

    head(path: string, handler?: string) {
        return this.route("HEAD", path, handler);
    }

    options(path: string, handler?: string) {
        return this.route("OPTIONS", path, handler);
    }

    async load() {
        for (const route of this.routes) {
            const handler = await this.loader.load(route.handler);

            this.server.route({
                method: route.method,
                url: route.path,
                handler
            });
        }
    }

    private pathToHandler(path: string) {
        const cleanPath = path
            .replace(/^\/+|\/+$/g, "")
            .replace(/\/+/g, "/");

        if (!cleanPath) {
            return "index";
        }

        return cleanPath;
    }
}