namespace Dima.Core.Common.Extensions;

public static class DateTimeExtension
{
    public static DateTime GetStartOfMonth(this DateTime date, int? month = null, int? year = null) =>
        new(year ?? date.Year, month ?? date.Month, 1);
    
    public static DateTime GetEndOfMonth(this DateTime date, int? month = null, int? year = null)
    => new DateTime(year ?? date.Year, month ?? date.Month, 1).AddMonths(1).AddDays(-1);
}