namespace Reservation.Domain
{
    // TODO: Move to shared project
    
    public static  class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
            => string.IsNullOrEmpty(str);
    }
}