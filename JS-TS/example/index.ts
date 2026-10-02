import Fastify from "fastify";
import { FastRouter } from "../src/index.js";

async function main() {
    const server = Fastify({
        logger: true
    });

    const router = new FastRouter(server, {
        directory: "./example/routes"
    });

    router.get("/hello");  //Automatically load Routes from the specified directory
    router.get("/users/me");

    await router.load();

    await server.listen({
        port: 3000,
        host: "127.0.0.1"
    });
}

main().catch(error => {
    console.error(error);
    process.exit(1);
});