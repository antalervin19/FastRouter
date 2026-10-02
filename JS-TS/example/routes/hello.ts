import type { FastifyRequest, FastifyReply } from "fastify";

export default async function (
    request: FastifyRequest,
    reply: FastifyReply
) {
    return reply.send({
        message: "Hello from FastRouter!"
    });
}