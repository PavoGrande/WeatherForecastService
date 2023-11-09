namespace WeatherForecast.Api.Contracts
{
    /// <summary>
    ///     Document data access exception type.
    /// </summary>
    public class DocumentDataAccessException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DocumentDataAccessException" /> class.
        /// </summary>
        /// ///
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DocumentDataAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}