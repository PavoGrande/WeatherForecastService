namespace WeatherForecast.Api.Storage
{
    /// <summary>
    ///     Options for PostgreSql.
    /// </summary>
    public class PostgreSqlOptions
    {
        public const string PostgreSql = "PostgreSqlOptions";

        /// <summary>
        ///     The database connection string.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}