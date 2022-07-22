using System.Net;

public class MinimaServer
{
    private HttpListener listener;
    private int port;

    public MinimaServer(int port)
    {
        this.port = port;
        listener = new HttpListener();
        listener.Prefixes.Add(String.Format("http://*:{0}/", port));
    }

    public void start()
    {
        Console.WriteLine(String.Format("Starting server at port {0}...", this.port));
        listener.Start();

        while (listener.IsListening)
        {
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            Console.WriteLine(String.Format("Got a new request: {0} {1} {2}", request.HttpMethod, request.Url, request.RawUrl));

            HttpListenerResponse response = context.Response;
            response.ContentType = "text/html";
            response.StatusCode = 200;

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes("<html><body>Hello world!</body></html>");
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }

    public void stop()
    {
        Console.WriteLine("Stopping server...");
        listener.Stop();
        Console.WriteLine("Server stopped.");
    }

    public void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        this.stop();
        Console.WriteLine("Exiting...");
    }
}
