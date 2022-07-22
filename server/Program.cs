MinimaServer server = new MinimaServer(8080);

Console.CancelKeyPress += new ConsoleCancelEventHandler(server.Console_CancelKeyPress);

server.setRoute("/", (request, response) =>
{
    response.StatusCode = 200;
    response.ContentType = "application/json";
    byte[] buffer = System.Text.Encoding.UTF8.GetBytes("{\"status\": \"ok\"}");
    response.OutputStream.Write(buffer, 0, buffer.Length);
});

server.setRoute("/error", (request, response) =>
{
    throw new Exception("Error");
});

server.start();
