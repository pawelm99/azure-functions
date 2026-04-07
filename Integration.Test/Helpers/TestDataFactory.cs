using System.Reflection;
using Integration.Application.Models;

namespace Integration.Test.Helpers;

/// <summary>
/// Helper class to create test instances of models with internal setters
/// </summary>
public static class TestDataFactory
{
    public static Invoice CreateInvoice(
        Guid? id = null,
        string? invoiceId = null,
        Customer? customer = null,
        Vendor? vendor = null,
        DateTimeOffset? invoiceDate = null,
        string? paymentTerm = null)
    {
        var invoice = new Invoice();
        SetProperty(invoice, nameof(Invoice.Id), id ?? Guid.NewGuid());
        SetProperty(invoice, nameof(Invoice.InvoiceId), invoiceId);
        SetProperty(invoice, nameof(Invoice.Customer), customer ?? CreateCustomer());
        SetProperty(invoice, nameof(Invoice.Vendor), vendor ?? CreateVendor());
        SetProperty(invoice, nameof(Invoice.InvoiceDate), invoiceDate);
        SetProperty(invoice, nameof(Invoice.PaymentTerm), paymentTerm);
        return invoice;
    }

    public static Invoice CreateInvoiceWithCurrency(
        Guid? id = null,
        string? invoiceId = null,
        Customer? customer = null,
        Vendor? vendor = null,
        DateTimeOffset? invoiceDate = null,
        double amount = 0,
        string currencyCode = "USD",
        string? paymentTerm = null)
    {
        var invoice = CreateInvoice(id, invoiceId, customer, vendor, invoiceDate, paymentTerm);

        var currencyValueType = typeof(Azure.AI.FormRecognizer.DocumentAnalysis.CurrencyValue);
        var currencyValue = Activator.CreateInstance(currencyValueType, nonPublic: true);

        if (currencyValue != null)
        {
            var boxed = (object)currencyValue;
            var amountField = currencyValueType.GetField("<Amount>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            var codeField = currencyValueType.GetField("<Code>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);

            amountField?.SetValue(boxed, amount);
            codeField?.SetValue(boxed, currencyCode);

            SetProperty(invoice, nameof(Invoice.InvoiceTotal),
                (Azure.AI.FormRecognizer.DocumentAnalysis.CurrencyValue?)boxed);
        }

        return invoice;
    }

    public static Customer CreateCustomer(
        Guid? id = null,
        string? name = null,
        string? taxId = null,
        string? addressRecipient = null)
    {
        var customer = new Customer
        {
            Id = id ?? Guid.NewGuid()
        };
        SetProperty(customer, nameof(Customer.Name), name);
        SetProperty(customer, nameof(Customer.TaxId), taxId);
        customer.AddressRecipient = addressRecipient;
        return customer;
    }

    public static Vendor CreateVendor(
        Guid? id = null,
        string? name = null,
        string? addressRecipient = null)
    {
        var vendor = new Vendor
        {
            Id = id ?? Guid.NewGuid()
        };
        SetProperty(vendor, nameof(Vendor.Name), name);
        SetProperty(vendor, nameof(Vendor.AddressRecipient), addressRecipient);
        return vendor;
    }

    private static void SetProperty<T>(object obj, string propertyName, T value)
    {
        var property = obj.GetType().GetProperty(propertyName);
        property?.SetValue(obj, value);
    }
}
