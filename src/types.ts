import type { FastifyReply, FastifyRequest } from "fastify";

export type RouteHandler = (
    request: FastifyRequest,
    reply: FastifyReply
) => unknown | Promise<unknown>;

export type HttpMethod =
    | "GET"
    | "POST"
    | "PUT"
    | "PATCH"
    | "DELETE"
    | "HEAD"
    | "OPTIONS";

export interface RouteDefinition {
    method: HttpMethod;
    path: string;
    handler: string;
}