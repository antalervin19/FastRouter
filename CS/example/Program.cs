using FastRouter;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var router = new FastRouter.FastRouter(app);   // defaults: namespace "Routes", entry assembly

router.Get("/hello");
router.Get("/users/me");
router.Get("/users/{id}");
router.Post("/users");

router.Load();

app.Run();
