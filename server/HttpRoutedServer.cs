using System.Net;

public class HttpRoutedServer
{
    private HttpListener listener;
    private string prefix;
    private IDictionary<String, Action<HttpListenerRequest, HttpWrappedResponse>> routes;

    public HttpRoutedServer(int port)
    {
        this.prefix = String.Format("http://*:{0}/", port);

        this.listener = new HttpListener();
        this.listener.Prefixes.Add(this.prefix);

        this.routes = new Dictionary<String, Action<HttpListenerRequest, HttpWrappedResponse>>();
    }

    public void start()
    {
        Console.WriteLine(String.Format("Starting server at prefix {0}", this.prefix));
        listener.Start();

        while (listener.IsListening)
        {
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpWrappedResponse response = new HttpWrappedResponse(context.Response);

            Console.WriteLine(String.Format("Got a new request: {0} {1} {2}", request.HttpMethod, request.Url, request.RawUrl));

            if (request.RawUrl == null)
            {
                Console.WriteLine(String.Format("Couldn't find request path, cannot determine route"));
                response.setStateDefaultNotFound();
            }
            else
            {
                try
                {
                    var handler = routes[request.RawUrl];
                    try
                    {
                        handler(request, response);
                    }
                    catch (Exception e)
                    {
                        response.setStateException(e);
                    }
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine(String.Format("Couldn't find route for request path: {0}", request.RawUrl));
                    response.setStateDefaultNotFound();
                }
            }

            response.close();
        }
    }

    public void stop()
    {
        Console.WriteLine("Stopping server");
        listener.Stop();
        Console.WriteLine("Server stopped");
    }

    public bool setRoute(string rawUrl, Action<HttpListenerRequest, HttpWrappedResponse> handler)
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
}
