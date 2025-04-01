using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Business.Managers;

public interface IWhatsAppSenderService
{
	Task<string> SendMessageAsync(string to, string message);
}

public class WhatsAppSenderService : IWhatsAppSenderService
{
	private readonly string _accountSid;
	private readonly string _authToken;
	private readonly string _fromNumber;

	public WhatsAppSenderService(string accountSid, string authToken, string fromNumber)
	{
		_accountSid = accountSid;
		_authToken = authToken;
		_fromNumber = fromNumber;
	}

	public async Task<string> SendMessageAsync(string to, string message)
	{
		TwilioClient.Init(_accountSid, _authToken);

		var messageResource = await MessageResource.CreateAsync(
			from: new PhoneNumber($"whatsapp:{_fromNumber}"),
			to: new PhoneNumber($"whatsapp:{to}"),
			body: message
		);

		return messageResource.Sid;
	}
}