HttpRoutedServer server = new HttpRoutedServer(8080);

Console.CancelKeyPress += new ConsoleCancelEventHandler((object? sender, ConsoleCancelEventArgs e) =>
{
    server.stop();
    Console.WriteLine("Exiting");
});

server.setRoute("/", (request, response) =>
{
    response.setStatusCode(200);
    response.setContentType("application/json");
    response.write("{\"status\": \"ok\"}");
});

server.setRoute("/error", (request, response) =>
{
    throw new Exception("Error");
});

server.start();
