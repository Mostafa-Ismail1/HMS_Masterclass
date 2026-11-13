using HMS.Domain.Common;

namespace HMS.Domain.Entities.Invoices;

public static class InvoiceErrors
{
    public static readonly Error InvalidAmount = Error.Validation(
        "Invoice.InvalidAmount",
        "Total amount must be greater than or equal to zero.");

    public static readonly Error AlreadyPaid = Error.Conflict(
        "Invoice.AlreadyPaid",
        "Invoice is already marked as paid.");

    public static readonly Error NotFound = Error.NotFound(
        "Invoice.NotFound",
        "Invoice was not found.");
}
