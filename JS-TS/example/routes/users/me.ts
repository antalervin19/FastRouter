import type { FastifyRequest, FastifyReply } from "fastify";

export default async function (
    request: FastifyRequest,
    reply: FastifyReply
) {
    return reply.send({
        id: 1,
        name: "FastRouter User"
    });
}