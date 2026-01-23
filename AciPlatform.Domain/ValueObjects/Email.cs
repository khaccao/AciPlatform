namespace AciPlatform.Domain.ValueObjects;

public record Email
{
    public string Value { get; init; }

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email không được để trống", nameof(value));

        if (!IsValidEmail(value))
            throw new ArgumentException("Email không hợp lệ", nameof(value));

        Value = value;
    }

    public static Email Create(string value) => new(value);

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public override string ToString() => Value;
}
