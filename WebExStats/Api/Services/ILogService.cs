using Stats.Models;

namespace Stats.Api.Services;

public interface ILogService
{
    Task<bool> LogAlert(AppLog appLog);
}