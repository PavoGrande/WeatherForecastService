namespace WeatherForecast.Api.Contracts
{
    /// <summary>
    ///     Document data access interface.
    /// </summary>
    public interface IDocumentDataAccess
    {
        /// <summary>
        ///     Writes a document to the implementation specific data store.
        /// </summary>
        /// <param name="document">The document to be written to the data store.</param>
        /// <param name="cancellationToken">
        ///     <see cref="CancellationToken" />.
        /// </param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <returns>A completed <see cref="Task" /> indicating the operation ran to completion.</returns>
        /// <exception cref="DocumentDataAccessException">Document already exists.</exception>
        /// <exception cref="DocumentDataAccessException">Unknown database error.</exception>
        Task AddDocumentAsync<TDocument>(TDocument document, CancellationToken cancellationToken) where TDocument : class;

        /// <summary>
        ///     Gets a document from the implementation specific data store.
        /// </summary>
        /// <param name="documentId">The id of the document to be retrieve from the data store.</param>
        /// <param name="cancellationToken">
        ///     <see cref="CancellationToken" />.
        /// </param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <returns>A document of type <see cref="TDocument" />.</returns>
        /// <exception cref="DocumentDataAccessException">Unknown database error.</exception>
        Task<TDocument> GetDocumentAsync<TDocument>(int documentId, CancellationToken cancellationToken) where TDocument : class;

        /// <summary>
        ///     Gets a list document from the implementation specific data store.
        /// </summary>
        /// <param name="cancellationToken">
        ///     <see cref="CancellationToken" />.
        /// </param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <returns>A <see cref="IReadOnlyList{TDocument}" />.</returns>
        /// <exception cref="DocumentDataAccessException">Unknown database error.</exception>
        Task<IReadOnlyList<TDocument>> GetDocumentsAsync<TDocument>(CancellationToken cancellationToken) where TDocument : class;

        /// <summary>
        ///     Remove a document from the implementation specific data store.
        /// </summary>
        /// <param name="documentId">The id of the document to remove from the data store.</param>
        /// <param name="cancellationToken">
        ///     <see cref="CancellationToken" />.
        /// </param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <returns>A completed <see cref="Task" /> indicating the operation ran to completion.</returns>
        /// <exception cref="DocumentDataAccessException">Unknown database error.</exception>
        Task DeleteDocumentAsync<TDocument>(int documentId, CancellationToken cancellationToken) where TDocument : class;
    }
}