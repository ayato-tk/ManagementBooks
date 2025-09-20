using System.Net;
using System.Net.Mail;
using GestaoLivros.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GestaoLivros.Application.Services.Implementations;

public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly string _smtpHost =
        configuration["EmailSettings:Host"] ?? throw new ArgumentNullException("EmailSettings:Host is not defined");

    private readonly int _smtpPort = configuration.GetValue<int>("EmailSettings:Port");

    private readonly bool _ssl = configuration.GetValue<bool>("EmailSettings:Ssl");

    private readonly string _smtpUser =
        configuration["EmailSettings:User"] ?? throw new ArgumentNullException("EmailSettings:User is not defined");

    private readonly string _smtpPass =
        configuration["EmailSettings:Pass"] ?? throw new ArgumentNullException("EmailSettings:Pass is not defined");

    private readonly string _from = configuration["EmailSettings:From"] ??
                                    throw new ArgumentNullException("EmailSettings:From is not defined");

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var message = new MailMessage();
        message.From = new MailAddress(_from);
        message.To.Add(to);
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        using var client = new SmtpClient(_smtpHost, _smtpPort);
        client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
        client.EnableSsl = _ssl;

        await client.SendMailAsync(message);
    }
}