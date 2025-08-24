using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendGrid;
using SendGrid.Helpers.Mail;

using IHost host = Host.CreateDefaultBuilder(args).Build();
IConfiguration configuration = host.Services.GetRequiredService<IConfiguration>();

string apiKey = configuration.GetValue<string>("SendGrid:ApiKey") ??
    throw new InvalidOperationException("SendGrid's API Key is not configured.");
string senderEmail = configuration.GetValue<string>("Sender:Email") ?? 
    throw new InvalidOperationException("Sender's email address is not configured.");
string senderName = configuration.GetValue<string>("Sender:Name") ?? 
    throw new InvalidOperationException("Sender's name is not configured.");
string recipientEmail = configuration.GetValue<string>("Recipient:Email") ?? 
    throw new InvalidOperationException("Recipient's email address is not configured.");
string recipientName = configuration.GetValue<string>("Recipient:Name") ?? 
    throw new InvalidOperationException("Recipient's name is not configured.");

SendGridClient client = new(apiKey);
SendGridMessage message = new()
{
    From = new EmailAddress(senderEmail, senderName),
    Subject = "Sending with Twilio SendGrid is Fun",
    PlainTextContent = "and easy to do anywhere, especially with C#",
};
message.AddTo(new EmailAddress(recipientEmail, recipientName));
Response response = await client.SendEmailAsync(message);

// A success status code means SendGrid received the email request and will process it.
// Errors can still occur when SendGrid tries to send the email.
// If email is not received, use this URL to debug: https://app.sendgrid.com/email_activity
Console.WriteLine(response.IsSuccessStatusCode ? "Email queued successfully!" : "Something went wrong!");
