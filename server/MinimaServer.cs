using System.Net;

public class MinimaServer
{
    private HttpListener listener;
    private int port;
    private IDictionary<String, Action<HttpListenerRequest, HttpListenerResponse>> routes;

    public MinimaServer(int port)
    {
        this.port = port;

        this.listener = new HttpListener();
        this.listener.Prefixes.Add(String.Format("http://*:{0}/", port));

        this.routes = new Dictionary<String, Action<HttpListenerRequest, HttpListenerResponse>>();
    }

    public void start()
    {
        Console.WriteLine(String.Format("Starting server at port {0}...", this.port));
        listener.Start();

        while (listener.IsListening)
        {
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            Console.WriteLine(String.Format("Got a new request: {0} {1} {2}", request.HttpMethod, request.Url, request.RawUrl));

            bool missingRoute = true;

            if (request.RawUrl != null)
            {
                try
                {
                    var handler = routes[request.RawUrl];
                    missingRoute = false;
                    try
                    {
                        handler(request, response);
                    }
                    catch (Exception e)
                    {
                        response.ContentType = "text/plain";
                        response.StatusCode = 500;

                        if (e.StackTrace != null)
                        {
                            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(e.StackTrace);
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
                catch (KeyNotFoundException) { }
            }

            if (missingRoute)
            {
                Console.WriteLine(String.Format("Couldn't find route for path: {0}", request.RawUrl));

                response.ContentType = "text/plain";
                response.StatusCode = 404;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes("not found");
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }

            response.OutputStream.Close();
        }
    }

    public bool setRoute(string rawUrl, Action<HttpListenerRequest, HttpListenerResponse> handler)
    {
        try
        {
            routes.Add(rawUrl, handler);
            return true;
        }
        catch (Exception)
        {
            return false;
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
