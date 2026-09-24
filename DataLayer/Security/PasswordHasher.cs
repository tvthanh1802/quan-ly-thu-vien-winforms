using System.Security.Cryptography;

namespace DataLayer.Security;

public sealed class PasswordHasher
{
    private const string Prefix = "PBKDF2-SHA256";
    public const int CurrentIterations = 120_000;
    private const int SaltSize = 16;
    private const int KeySize = 32;

    public string Hash(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] key = Rfc2898DeriveBytes.Pbkdf2(password, salt, CurrentIterations,
            HashAlgorithmName.SHA256, KeySize);
        return $"{Prefix}${CurrentIterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(key)}";
    }

    public bool Verify(string password, string encoded)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(encoded)) return false;
        string[] parts = encoded.Split('$');
        if (parts.Length != 4 || !string.Equals(parts[0], Prefix, StringComparison.Ordinal)
            || !int.TryParse(parts[1], out int iterations) || iterations <= 0) return false;
        try
        {
            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] expected = Convert.FromBase64String(parts[3]);
            byte[] actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations,
                HashAlgorithmName.SHA256, expected.Length);
            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
        catch (FormatException) { return false; }
    }

    public bool IsHash(string? value) => value?.StartsWith(Prefix + "$", StringComparison.Ordinal) == true;

    public bool NeedsRehash(string? encoded)
    {
        if (!IsHash(encoded)) return true;
        string[] parts = encoded!.Split('$');
        return parts.Length != 4 || !int.TryParse(parts[1], out int iterations)
            || iterations < CurrentIterations;
    }
}

