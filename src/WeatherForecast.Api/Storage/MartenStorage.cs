using Ardalis.GuardClauses;
using Marten;
using Marten.Exceptions;
using WeatherForecast.Api.Contracts;

namespace WeatherForecast.Api.Storage
{
    /// <summary>
    ///     Marten database specific implementation of <see cref="IDocumentDataAccess" />.
    /// </summary>
    public class MartenStorage : IDocumentDataAccess
    {
        private readonly IDocumentSession _documentSession;
        private readonly ILogger<IDocumentDataAccess> _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MartenStorage" /> class.
        /// </summary>
        /// <param name="documentSession"><see cref="IDocumentSession" />.</param>
        /// <param name="logger">The logger.</param>
        public MartenStorage(IDocumentSession documentSession, ILogger<IDocumentDataAccess> logger)
        {
            Guard.Against.Null(documentSession, nameof(IDocumentSession));
            Guard.Against.Null(logger, nameof(ILogger<IDocumentDataAccess>));

            _documentSession = documentSession;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task AddDocumentAsync<TDocument>(TDocument document, CancellationToken cancellationToken) where TDocument : class
        {
            Guard.Against.Null(document, nameof(document));

            try
            {
                _documentSession.Insert(document);
                await _documentSession.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                throw GetDocumentDataAccessException(exception);
            }
        }

        /// <inheritdoc />
        public async Task<TDocument> GetDocumentAsync<TDocument>(int documentId, CancellationToken cancellationToken) where TDocument : class
        {
            try
            {
                return await _documentSession.LoadAsync<TDocument>(documentId, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                throw GetDocumentDataAccessException(exception);
            }
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TDocument>> GetDocumentsAsync<TDocument>(CancellationToken cancellationToken) where TDocument : class
        {
            try
            {
                var docs = _documentSession.Query<TDocument>().ToListAsync(cancellationToken);

                return await docs;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                throw GetDocumentDataAccessException(exception);
            }
        }

        /// <inheritdoc />
        public async Task DeleteDocumentAsync<TDocument>(int documentId, CancellationToken cancellationToken) where TDocument : class
        {
            try
            {
                _documentSession.Delete<TDocument>(documentId);
                await _documentSession.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                throw GetDocumentDataAccessException(exception);
            }
        }

        private static DocumentDataAccessException GetDocumentDataAccessException(Exception exception)
        {
            return exception switch
            {
                InvalidOperationException when exception.Message == "Id/id values cannot be null or empty" => new DocumentDataAccessException(DocumentDataAccessConstants.IdNotNullOrEmpty, exception),
                DocumentAlreadyExistsException => new DocumentDataAccessException(DocumentDataAccessConstants.DocumentAlreadyExists, exception),
                _ => new DocumentDataAccessException(DocumentDataAccessConstants.UnHandledException, exception)
            };
        }
    }
}