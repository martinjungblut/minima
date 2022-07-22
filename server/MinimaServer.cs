using System;
using System.Net;

public class MinimaServer {
    private HttpListener listener;

    public MinimaServer(int port) {
        listener = new HttpListener();
        listener.Prefixes.Add(String.Format("http://*:{0}/", port));
    }

    public void stop() {
        Console.WriteLine("Stopping server...");
        listener.Stop();
        Console.WriteLine("Server stopped.");
    }

    public void start() {
        Console.WriteLine("Starting server...");
        listener.Start();

        while (listener.IsListening) {
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            Console.WriteLine(String.Format("Got a new request with method: {0} {1}", request.HttpMethod, request.Url));

            HttpListenerResponse response = context.Response;

            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }

    public void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e) {
        this.stop();
        Console.WriteLine("Exiting...");
    }
}