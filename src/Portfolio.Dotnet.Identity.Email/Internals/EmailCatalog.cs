namespace Portfolio.Dotnet.Identity.Email.Internals
{
    internal static class EmailCatalog
    {
        public static EmailMetadata AccountCreated { get; } = new EmailMetadata("Account Created", @"Emails/AccountCreated/AccountCreated.cshtml");
        public static EmailMetadata ResetPassword { get; } = new EmailMetadata("Password Reset", @"Emails/ResetPassword/ResetPassword.cshtml");
        public static EmailMetadata PasswordChanged { get; } = new EmailMetadata("Password Changed", @"Emails/PasswordChanged/PasswordChanged.cshtml");
    }
}
