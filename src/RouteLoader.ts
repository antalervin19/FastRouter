import fs from "node:fs";
import path from "node:path";
import { pathToFileURL } from "node:url";
import type { RouteHandler } from "./types.js";

export class RouteLoader {
    constructor(private readonly directory: string) {}

    async load(handlerPath: string): Promise<RouteHandler> {
        const basePath = path.resolve(
            this.directory,
            handlerPath
        );

        const extensions = [
            ".js",
            ".ts",
            ".mjs",
            ".cjs"
        ];

        for (const extension of extensions) {
            const filePath = `${basePath}${extension}`;

            if (!fs.existsSync(filePath)) {
                continue;
            }

            const module = await import(
                pathToFileURL(filePath).href
            );

            if (typeof module.default !== "function") {
                throw new Error(
                    `Route "${handlerPath}" must export a default function`
                );
            }

            return module.default;
        }

        throw new Error(
            `Route handler not found: ${handlerPath}`
        );
    }
}