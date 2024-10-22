﻿//#define SIMPLE

using Microsoft.Extensions.Options;

namespace ApplicationConfiguration.Reload;
#if SIMPLE
public class NotificationService
{
    private EmailOptions _emailOptions;
    private readonly ILogger _logger;

    public NotificationService(IOptionsMonitor<EmailOptions> emailOptionsMonitor, ILogger<NotificationService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        ArgumentNullException.ThrowIfNull(emailOptionsMonitor);
        _emailOptions = emailOptionsMonitor.CurrentValue;
    }

    public Task NotifyAsync(string to)
    {
        _logger.LogInformation(
            "Notification sent by '{SenderEmailAddress}' to '{to}'.",
            _emailOptions.SenderEmailAddress,
            to
        );
        return Task.CompletedTask;
    }
}
#else
public class NotificationService
{
	private readonly ILogger _logger;
	private readonly IOptionsMonitor<EmailOptions> _monitor;
	private EmailOptions _emailOptions;
	private IDisposable? _onChangeListener;

	public NotificationService(IOptionsMonitor<EmailOptions> emailOptionsMonitor, ILogger<NotificationService> logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		ArgumentNullException.ThrowIfNull(emailOptionsMonitor);
		_monitor = emailOptionsMonitor;
		_emailOptions = emailOptionsMonitor.CurrentValue;
		StartListeningForChanges();
	}

	public Task NotifyAsync(string to)
	{
		_logger.LogInformation(
				"Notification sent by '{SenderEmailAddress}' to '{to}'. ({monitor})",
				_emailOptions.SenderEmailAddress,
				to,
				_monitor.CurrentValue.SenderEmailAddress
			);
		return Task.CompletedTask;
	}

	public void StartListeningForChanges()
	{
		_onChangeListener = _monitor.OnChange(options =>
		{
			if (_emailOptions?.SenderEmailAddress != options.SenderEmailAddress)
			{
				_logger.LogInformation(
						"EmailOptions changed from {old} to {new}.",
						_emailOptions?.SenderEmailAddress,
						options.SenderEmailAddress
					);
				_emailOptions = options;
			}
		});
	}

	public void StopListeningForChanges()
	{
		_onChangeListener?.Dispose();
	}
}
#endif

public class EmailOptions
{
	public string? SenderEmailAddress { get; set; }
}

public static class NotificationServiceStartupExtensions
{
	public static WebApplicationBuilder AddNotificationService(this WebApplicationBuilder builder)
	{
		builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(nameof(EmailOptions)));
		builder.Services.AddSingleton<NotificationService>();
		return builder;
	}

	public static WebApplication MapNotificationService(this WebApplication app)
	{
		app.MapPost("notify/{email}", async (string email, NotificationService service) =>
		{
			await service.NotifyAsync(email);
			return Results.Ok();
		});
		app.MapPut("notify", (NotificationService service) =>
		{
			service.StartListeningForChanges();
			return Results.Ok();
		});
		app.MapDelete("notify", (NotificationService service) =>
		{
			service.StopListeningForChanges();
			return Results.Ok();
		});
		return app;
	}
}