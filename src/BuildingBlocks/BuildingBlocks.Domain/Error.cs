namespace BuildingBlocks.Domain
{
    // TODO: Add Code property to Error
    // so that Errors can be identified by some criteria

    public record Error(string Message)
    {
        public static implicit operator Error(string message) => new(message);
    };
}