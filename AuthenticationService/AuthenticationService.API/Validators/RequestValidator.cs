using Ganss.Xss;

namespace AuthenticationService.API.Validators
{
    public class RequestValidator
    {
        private readonly HtmlSanitizer _sanitizer;

        public RequestValidator()
        {
            _sanitizer = new HtmlSanitizer(); // Initialize once
        }

        public void ValidateInt(string propertyName, int input)
        {
            if (input < 0)
            {
                throw new ArgumentException($"The property '{propertyName}' cannot be negative.");
            }
        }

        public void ValidateString(string propertyName, string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException($"The property '{propertyName}' cannot be null or empty.");
            }

            if (input.Length > 500)
            {
                throw new ArgumentException($"The property '{propertyName}' cannot exceed 500 characters.");
            }

            if (input != System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(input)))
            {
                throw new ArgumentException($"The property '{propertyName}' must be UTF-8 encoded.");
            }

            if (input != _sanitizer.Sanitize(input))
            {
                throw new ArgumentException($"The property '{propertyName}' contains invalid HTML tags.");
            }

            string[] shellCharacters = { ";", "|", "&", "$", "<", ">" };
            foreach (string character in shellCharacters)
            {
                if (input.Contains(character))
                {
                    throw new ArgumentException($"The property '{propertyName}' contains unsafe shell characters.");
                }
            }

            if (input.Contains("../") || input.Contains(@"..\"))
            {
                throw new ArgumentException($"The property '{propertyName}' contains path traversal characters.");
            }

            string[] controlCharacters = { "\0", "\a", "\b", "\f", "\v", "\x1b", "\x1c", "\x1d", "\x1e", "\x1f" };
            foreach (string character in controlCharacters)
            {
                if (input.Contains(character))
                {
                    throw new ArgumentException($"The property '{propertyName}' contains control characters.");
                }
            }

            if (input.Contains("javascript:"))
            {
                throw new ArgumentException($"The property '{propertyName}' contains unsafe javascript: schemes.");
            }
        }
    }
}
