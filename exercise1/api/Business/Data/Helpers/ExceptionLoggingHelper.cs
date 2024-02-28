using MediatR;
using StargateAPI.Business.Commands;
using System;

namespace StargateAPI.Business.Data.Helpers
{
    public class ExceptionLoggingHelper : IExceptionLoggingHelper
    {
        private readonly StargateContext _context;
        private readonly ILogger<ExceptionLoggingHelper> _logger;

        public ExceptionLoggingHelper(StargateContext context, ILogger<ExceptionLoggingHelper> logger)
        {
            _context = context;
            _logger = logger;
        }
        //Virtual Wrapper Method for unit testing and action protection
        public virtual void PersistException(Exception exception)
        {
            this.StoreException(exception);
        }
        private async void StoreException(Exception exception) {
            var error = new Error()
            {
               Message = exception.Message,
               Created = DateTime.UtcNow
            };

            _logger.LogInformation($"Persisting error {error.Message}");
            await _context.Errors.AddAsync(error);

            await _context.SaveChangesAsync();
        }
    }
}
