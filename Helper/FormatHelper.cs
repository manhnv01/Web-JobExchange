namespace JobExchange.Helper
{
    public static class FormatHelper
    {
        public static string FormatCurrency(int? amount)
        {
            if (amount.HasValue)
            {
                return amount.Value.ToString("N0") + " VND";
            }
            return "0 VND";
        }

        public static int GetRemainingTime(DateTime? expirationDate)
        {
            if (expirationDate.HasValue)
            {
                var remainingTime = expirationDate.Value - DateTime.Now;
                return remainingTime.Days;
            }
            return 0;
        }
    }
}