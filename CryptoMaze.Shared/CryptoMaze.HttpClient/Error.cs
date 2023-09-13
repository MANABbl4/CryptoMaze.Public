using System.Net;

namespace CryptoMaze.Client
{
    public class Error
    {
        /// <summary>
        /// The error code from the server
        /// </summary>
        public HttpStatusCode Code { get; set; }

        /// <summary>
        /// The message for the error that occurred
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public Error(HttpStatusCode code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Code}: {Message}";
        }
    }
}
