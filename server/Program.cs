MinimaServer server = new MinimaServer(8080);
Console.CancelKeyPress += new ConsoleCancelEventHandler(server.Console_CancelKeyPress);
server.start();