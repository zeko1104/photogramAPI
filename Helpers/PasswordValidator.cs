using System.Text.RegularExpressions;

namespace PhotogramAPI.Helpers
{
    public static class PasswordValidator
    {
        public static bool IsStrongPassword(string password)
        {
            // Minimum 8 character, at least 1 uppercase, 1 lowercase, 1 digit, 1 special char
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#._-])[A-Za-z\d@$!%*?&#._-]{8,}$");

            return regex.IsMatch(password);
        }
    }
}
