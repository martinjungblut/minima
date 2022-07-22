using System.Net;

public class HttpWrappedResponse
{
    private HttpListenerResponse response;

    public HttpWrappedResponse(HttpListenerResponse response)
    {
        this.response = response;
    }

    public void setStatusCode(int statusCode)
    {
        this.response.StatusCode = statusCode;
    }

    public void setContentType(string contentType)
    {
        this.response.ContentType = contentType;
    }

    public void write(string content)
    {
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);
        this.response.OutputStream.Write(buffer, 0, buffer.Length);
    }

    public void close()
    {
        this.response.OutputStream.Close();
    }

    public void setStateDefaultNotFound()
    {
        this.setStatusCode(404);
        this.setContentType("text/plain");
        this.write("not found");
    }

    public void setStateException(Exception e)
    {
        this.setStatusCode(500);
        this.setContentType("text/plain");

        if (e.StackTrace != null)
        {
            this.write(e.StackTrace);
        }
    }
}
