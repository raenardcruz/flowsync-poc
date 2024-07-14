using Builder.Models;
using Newtonsoft.Json.Linq;

namespace Builder.Steps
{
    public class ApiStep
    {
        private static readonly HttpClient client = new HttpClient();
        public static async Task<ApiStepResponse> CallApi(string url, string method, JToken headers = null, JToken payload = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(method), url);

            string contentType = "application/json"; // Default to JSON

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    string headerName = ((JProperty)header).Name.ToString();
                    string headerValue = ((JProperty)header).Value.ToString();
                    if (headerName.ToLower() != "content-type")
                        request.Headers.Add(headerName, headerValue);
                    if (headerName.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                    {
                        contentType = headerValue;
                    }
                }
            }

            if (payload != null)
            {
                if (contentType.Contains("application/json"))
                {
                    request.Content = new StringContent(payload.ToString(), System.Text.Encoding.UTF8, "application/json");
                }
                else if (contentType.Contains("application/x-www-form-urlencoded"))
                {
                    var urlEncodedContent = new FormUrlEncodedContent(payload.ToObject<Dictionary<string, string>>());
                    request.Content = urlEncodedContent;
                }
                else if (contentType.Contains("multipart/form-data"))
                {
                    var formDataContent = new MultipartFormDataContent();
                    foreach (var item in payload)
                    {
                        formDataContent.Add(new StringContent(item["value"].ToString()), item["name"].ToString());
                    }
                    request.Content = formDataContent;
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported Content-Type: {contentType}");
                }
            }

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                return new ApiStepResponse()
                {
                    Response = responseData,
                    StatusCode = response.StatusCode.GetHashCode()
                };
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                return new ApiStepResponse()
                {
                    Response = errorContent,
                    StatusCode = response.StatusCode.GetHashCode()
                };
            }
        }
    }
}
