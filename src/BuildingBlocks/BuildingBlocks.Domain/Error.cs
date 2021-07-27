namespace BuildingBlocks.Domain
{
    public record Error(string Message)
    {
        public static implicit operator Error(string message)
        {
            return new(message);
        }
    }
}