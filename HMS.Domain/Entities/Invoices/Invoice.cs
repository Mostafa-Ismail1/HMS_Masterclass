using HMS.Domain.Common;

namespace HMS.Domain.Entities.Invoices;

public class Invoice
{
    private Invoice(int patientId, int? admissionId, decimal totalAmount, DateOnly issueDate)
    {
        PatientId = patientId;
        AdmissionId = admissionId;
        TotalAmount = totalAmount;
        IssueDate = issueDate;
        Status = "Unpaid";
    }

    private Invoice()
    {
    }

    public int InvoiceId { get; private set; }
    public DateOnly IssueDate { get; private set; }
    public int PatientId { get; private set; }
    public int? AdmissionId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; } = null!;

    public static Result<Invoice> Create(int patientId, int? admissionId, decimal totalAmount, DateOnly issueDate)
    {
        if (totalAmount < 0)
        {
            return Result.Failure<Invoice>(InvoiceErrors.InvalidAmount);
        }

        return Result.Success(new Invoice(patientId, admissionId, totalAmount, issueDate));
    }

    public Result MarkAsPaid()
    {
        if (Status == "Paid")
        {
            return Result.Failure(InvoiceErrors.AlreadyPaid);
        }

        Status = "Paid";
        return Result.Success();
    }
}
