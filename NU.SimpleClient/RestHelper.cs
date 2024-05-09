using Avalonia;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NU.SimpleClient
{
    public class RestHelper
    {
        public static async Task PostRequest(string baseUrl, string relativeUrl, Func<HttpRequestMessage, Task> onRequest, Func<HttpResponseMessage, Task> onResponse, Dictionary<string, string> headers = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(baseUrl), relativeUrl));

                    if (headers != null)
                        foreach (var item in headers)
                        {
                            request.Headers.Add(item.Key, item.Value);
                        }

                    await onRequest(request);

                    using var response = await client.SendAsync(request);

                    await onResponse(response);
                }
            }
            catch (Exception ex)
            {
                await onResponse(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadGateway, Content = new StringContent(ex.ToString()) });
            }
        }
    }

    public static class RestHelperExtensions
    {
        public static StringContent SetJsonContent(this HttpRequestMessage request, object value)
        {
            request.Content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

            return (StringContent)request.Content;
        }

        public static MultipartFormDataContent InitializeMultipart(this HttpRequestMessage request)
        {
            var formContent = new MultipartFormDataContent();

            request.Content = formContent;

            return formContent;
        }

        public static MultipartFormDataContent SetFileArrayContent(this MultipartFormDataContent content, string field, IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                content.Add(new StreamContent(File.OpenRead(path)), field, Path.GetFileName(path));

            }

            return content;
        }
    }
}
