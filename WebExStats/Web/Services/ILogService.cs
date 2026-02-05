using Stats.Models;

namespace Stats.Web.Services;

public interface ILogService
{
    Task<bool> LogAlert(AppLog appLog);
}