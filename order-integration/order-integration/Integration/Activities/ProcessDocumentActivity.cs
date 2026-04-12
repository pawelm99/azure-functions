using Integration.Application.Models;
using Integration.Domain.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Integration.Activities;

public class ProcessDocumentActivity
{
    private readonly ILogger _logger;
    private readonly IDocumentService _documentService;


    public ProcessDocumentActivity(IDocumentService documentService, ILoggerFactory loggerFactory)
    {
        _documentService = documentService;
        _logger = loggerFactory.CreateLogger<ProcessDocumentActivity>();
    }


    [Function(nameof(ProcessDocumentActivity))]
    public async Task<List<Invoice>?> Run([ActivityTrigger] string message)
        => await _documentService.ExtractInvoiceFromPdfAsync(message);
}