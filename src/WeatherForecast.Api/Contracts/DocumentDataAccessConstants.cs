namespace WeatherForecast.Api.Contracts
{
    /// <summary>
    ///     Document data access literal messages.
    /// </summary>
    public static class DocumentDataAccessConstants
    {
        /// <summary>
        ///     The message to return when a document already exists in the database.
        /// </summary>
        public const string DocumentAlreadyExists = "Document already exists.";

        /// <summary>
        ///     The message to return when a document does not specify an Id value.
        /// </summary>
        public const string IdNotNullOrEmpty = "Id value cannot be null or empty";

        /// <summary>
        ///     The message to return when an unhandled exception occurs.
        /// </summary>
        public const string UnHandledException = "An unhandled exception occurred.";
    }
}