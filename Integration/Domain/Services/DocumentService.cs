using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Integration.Application.Models;
using Integration.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Integration.Domain.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public DocumentService(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<DocumentService>();
            _configuration = configuration;
        }


        public async Task<List<Invoice>?> ExtractInvoiceFromPdfAsync(string message)
        {
            var fileBytes = Convert.FromBase64String(message);
            Console.WriteLine($"Read file from message: {fileBytes}");
            _logger.LogInformation($"Read file from message: {fileBytes}");

            using MemoryStream stream = new(fileBytes);

            var client = new DocumentAnalysisClient(new Uri(_configuration["Values:FormRecognizerUrl"]!), new AzureKeyCredential(_configuration["Values:FormRecognizerKey"]!));
            var poller = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", stream);
            var result = poller?.Value;
            if (result == null || result?.Documents == null) return null;

            Console.WriteLine("AI read document");
            _logger.LogInformation($"AI read document");

            var invoices = new List<Invoice>();
            for (int i = 0; i < result.Documents.Count; i++)
            {
                var invoice = new Invoice();
                var customer = new Customer();
                var vendor = new Vendor();

                Console.WriteLine($"Document {i}:");
                AnalyzedDocument document = result.Documents[i];

                Console.WriteLine("Set invoice, customer, vendor");
                _logger.LogInformation($"Set invoice, customer, vendort");

                customer.Name = GetFieldByName("CustomerName", DocumentFieldType.String, document)?.AsString();
                customer.Address = MapAddress(GetFieldByName("CustomerAddress", DocumentFieldType.Address, document)?.AsAddress());
                customer.AddressRecipient = GetFieldByName("CustomerAddressRecipient", DocumentFieldType.String, document)?.AsString();
                customer.TaxId = GetFieldByName("CustomerTaxId", DocumentFieldType.String, document)?.AsString();

                invoice.InvoiceDate = GetFieldByName("InvoiceDate", DocumentFieldType.Date, document)?.AsDate();
                invoice.InvoiceId = GetFieldByName("InvoiceId", DocumentFieldType.String, document)?.AsString();
                invoice.InvoiceTotal = MapCurrency(GetFieldByName("InvoiceTotal", DocumentFieldType.Currency, document)?.AsCurrency());
                invoice.PaymentTerm = GetFieldByName("PaymentTerm", DocumentFieldType.String, document)?.AsString();
                invoice.SubTotal = MapCurrency(GetFieldByName("SubTotal", DocumentFieldType.Currency, document)?.AsCurrency());
                invoice.TotalTax = MapCurrency(GetFieldByName("TotalTax", DocumentFieldType.Currency, document)?.AsCurrency());
                invoice.PaymentDetails = MapPaymentDetails(GetFieldByName("PaymentDetails", DocumentFieldType.List, document)?.AsList());
                invoice.Items = GetItems("Items", document);

                vendor.Address = MapAddress(GetFieldByName("VendorAddress", DocumentFieldType.Address, document)?.AsAddress());
                vendor.AddressRecipient = GetFieldByName("VendorAddressRecipient", DocumentFieldType.String, document)?.AsString();
                vendor.Name = GetFieldByName("VendorName", DocumentFieldType.String, document)?.AsString();
                vendor.TaxId = GetFieldByName("VendorTaxId", DocumentFieldType.String, document)?.AsString();

                invoice.Customer = customer;
                invoice.Vendor = vendor;

                invoices.Add(invoice);
            }
            return invoices;
        }


        private static DocumentFieldValue? GetFieldByName(string fieldName, DocumentFieldType type, AnalyzedDocument document)
        {
            if (document.Fields.TryGetValue(fieldName, out DocumentField? field))
            {
                if (field.FieldType == type)
                {
                    Console.WriteLine($"Readed file: {field.Value} of type {type}.");
                    return field.Value;
                }
            }
            return default;
        }


        private static Address? MapAddress(AddressValue? addressValue)
        {
            if (addressValue == null) return null;

            return new Address
            {
                StreetAddress = addressValue.StreetAddress,
                City = addressValue.City,
                State = addressValue.State,
                PostalCode = addressValue.PostalCode,
                CountryRegion = addressValue.CountryRegion,
                Road = addressValue.Road,
                HouseNumber = addressValue.HouseNumber,
                Unit = addressValue.Unit,
                CityDistrict = addressValue.CityDistrict,
                StateDistrict = addressValue.StateDistrict,
                Suburb = addressValue.Suburb,
                House = addressValue.House,
                Level = addressValue.Level,
                PoBox = addressValue.PoBox
            };
        }


        private static Currency? MapCurrency(CurrencyValue? currencyValue)
        {
            if (currencyValue == null) return null;

            return new Currency
            {
                Amount = currencyValue.Value.Amount,
                CurrencySymbol = currencyValue.Value.Symbol,
                CurrencyCode = currencyValue.Value.Code
            };
        }


        private static List<PaymentDetail>? MapPaymentDetails(IReadOnlyList<DocumentField>? fields)
        {
            if (fields == null) return null;

            var paymentDetails = new List<PaymentDetail>();
            foreach (var field in fields)
            {
                if (field.FieldType == DocumentFieldType.Dictionary)
                {
                    var dict = field.Value.AsDictionary();
                    var detail = new PaymentDetail();

                    if (dict.TryGetValue("IBAN", out var iban) && iban.FieldType == DocumentFieldType.String)
                        detail.IBAN = iban.Value.AsString();

                    if (dict.TryGetValue("SWIFT", out var swift) && swift.FieldType == DocumentFieldType.String)
                        detail.SWIFT = swift.Value.AsString();

                    if (dict.TryGetValue("BankName", out var bankName) && bankName.FieldType == DocumentFieldType.String)
                        detail.BankName = bankName.Value.AsString();

                    paymentDetails.Add(detail);
                }
            }
            return paymentDetails;
        }


        private static List<Item> GetItems(string fieldName, AnalyzedDocument document)
        {
            var items = new List<Item>();
            if (document.Fields.TryGetValue(fieldName, out DocumentField? field))
            {
                if (field.FieldType == DocumentFieldType.List)
                {
                    foreach (DocumentField itemField in field.Value.AsList())
                    {
                        var item = new Item();
                        if (itemField.FieldType == DocumentFieldType.Dictionary)
                        {
                            IReadOnlyDictionary<string, DocumentField> itemFields = itemField.Value.AsDictionary();

                            if (itemFields.TryGetValue("Description", out DocumentField? itemDescriptionField))
                            {
                                if (itemDescriptionField.FieldType == DocumentFieldType.String)
                                {
                                    item.Description = itemDescriptionField.Value.AsString();
                                }
                            }

                            if (itemFields.TryGetValue("Amount", out DocumentField? itemAmountField))
                            {
                                if (itemAmountField.FieldType == DocumentFieldType.Currency)
                                {
                                    item.Amount = MapCurrency(itemAmountField.Value.AsCurrency());
                                }
                            }
                            items.Add(item);
                        }

                    }
                }
            }
            return items;
        }
    }
}
