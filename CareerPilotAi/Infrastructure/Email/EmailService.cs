
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CareerPilotAi.Infrastructure.Email;

public class EmailService
{
    private readonly ISendGridClient _sendGridClient;
    private readonly IOptions<SendGridAppSettings> _sendgridOptions;

    public EmailService(ISendGridClient sendGridClient, IOptions<SendGridAppSettings> sendgridOptions)
    {
        _sendGridClient = sendGridClient;
        _sendgridOptions = sendgridOptions;
    }

    public async Task SendRegistrationVerificationEmailAsync(string toEmail, string link, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
            throw new ArgumentNullException(nameof(toEmail), "Recipient email cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(link))
            throw new ArgumentNullException(nameof(link), "Verification link cannot be null or empty.");


        ValidateSendGridSettings();

        var from = new EmailAddress(_sendgridOptions.Value.FromEmail, "CareerPilotAi");
        var to = new EmailAddress(toEmail);
        var emailMessage = MailHelper
            .CreateSingleTemplateEmail(from, to, _sendgridOptions.Value.RegistrationVerificationTemplateId, new { LINK = link });

        var response = await _sendGridClient.SendEmailAsync(emailMessage, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Body.ReadAsStringAsync(cancellationToken);
            throw new Exception($"Failed to send email: {errorMessage}");
        }
    }

    private void ValidateSendGridSettings()
    {
        if (string.IsNullOrWhiteSpace(_sendgridOptions.Value.FromEmail))
        {
            throw new InvalidOperationException("FromEmail is not configured in SendGrid settings.");
        }

        if (string.IsNullOrWhiteSpace(_sendgridOptions.Value.RegistrationVerificationTemplateId))
        {
            throw new InvalidOperationException("RegistrationVerificationTemplateId is not configured in SendGrid settings.");
        }
    }
}