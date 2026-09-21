import path from "node:path";
import { pathToFileURL } from "node:url";
import type { RouteHandler } from "./types.js";

export class RouteLoader {
    constructor(private readonly directory: string) {}

    async load(handlerPath: string): Promise<RouteHandler> {
        const filePath = path.resolve(
            this.directory,
            `${handlerPath}.ts`
        );

        const module = await import(pathToFileURL(filePath).href);

        if (typeof module.default !== "function") {
            throw new Error(
                `Route "${handlerPath}" must export a default function`
            );
        }

        return module.default;
    }
}