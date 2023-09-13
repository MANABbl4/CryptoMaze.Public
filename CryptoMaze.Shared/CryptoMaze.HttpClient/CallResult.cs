using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;

namespace CryptoMaze.Client
{
    public class CallResult
    {
        /// <summary>
        /// An error if the call didn't succeed, will always be filled if Success = false
        /// </summary>
        public Error Error { get; set; }

        /// <summary>
        /// Whether the call was successful
        /// </summary>
        public bool Success => Error == null;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="error"></param>
        public CallResult(Error error)
        {
            Error = error;
        }

        /// <summary>
        /// Overwrite bool check so we can use if(callResult) instead of if(callResult.Success)
        /// </summary>
        /// <param name="obj"></param>
        public static implicit operator bool(CallResult obj)
        {
            return obj?.Success == true;
        }
    }

    /// <summary>
    /// The result of a request
    /// </summary>
    public class WebCallResult : CallResult
    {
        /// <summary>
        /// The request http method
        /// </summary>
        public HttpMethod RequestMethod { get; set; }

        /// <summary>
        /// The headers sent with the request
        /// </summary>
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> RequestHeaders { get; set; }

        /// <summary>
        /// The url which was requested
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// The body of the request
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// The status code of the response. Note that a OK status does not always indicate success, check the Success parameter for this.
        /// </summary>
        public HttpStatusCode? ResponseStatusCode { get; set; }

        /// <summary>
        /// The response headers
        /// </summary>
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> ResponseHeaders { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="responseHeaders"></param>
        /// <param name="requestUrl"></param>
        /// <param name="requestBody"></param>
        /// <param name="requestMethod"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="error"></param>
        public WebCallResult(
            HttpStatusCode? code,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> responseHeaders,
            string requestUrl,
            string requestBody,
            HttpMethod requestMethod,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> requestHeaders,
            Error error) : base(error)
        {
            ResponseStatusCode = code;
            ResponseHeaders = responseHeaders;

            RequestUrl = requestUrl;
            RequestBody = requestBody;
            RequestHeaders = requestHeaders;
            RequestMethod = requestMethod;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="error"></param>
        public WebCallResult(Error error) : base(error) { }

        /// <summary>
        /// Return the result as an error result
        /// </summary>
        /// <param name="error">The error returned</param>
        /// <returns></returns>
        public WebCallResult AsError(Error error)
        {
            return new WebCallResult(ResponseStatusCode, ResponseHeaders, RequestUrl, RequestBody, RequestMethod, RequestHeaders, error);
        }
    }

    /// <summary>
    /// The result of a request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WebCallResult<T> : CallResult
    {
        /// <summary>
        /// The data returned by the call, only available when Success = true
        /// </summary>
        public T Data { get; internal set; }

        /// <summary>
        /// The request http method
        /// </summary>
        public HttpMethod RequestMethod { get; set; }

        /// <summary>
        /// The headers sent with the request
        /// </summary>
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> RequestHeaders { get; set; }

        /// <summary>
        /// The url which was requested
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// The body of the request
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// The status code of the response. Note that a OK status does not always indicate success, check the Success parameter for this.
        /// </summary>
        public HttpStatusCode? ResponseStatusCode { get; set; }

        /// <summary>
        /// The response headers
        /// </summary>
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> ResponseHeaders { get; set; }

        /// <summary>
        /// Create a new result
        /// </summary>
        /// <param name="code"></param>
        /// <param name="responseHeaders"></param>
        /// <param name="requestUrl"></param>
        /// <param name="requestBody"></param>
        /// <param name="requestMethod"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="data"></param>
        /// <param name="error"></param>
        public WebCallResult(
            HttpStatusCode? code,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> responseHeaders,
            string requestUrl,
            string requestBody,
            HttpMethod requestMethod,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> requestHeaders,
            T data,
            Error error) : base(error)
        {
            ResponseStatusCode = code;
            ResponseHeaders = responseHeaders;

            RequestUrl = requestUrl;
            RequestBody = requestBody;
            RequestMethod = requestMethod;
            RequestHeaders = requestHeaders;
            Data = data;
        }
    }
}
