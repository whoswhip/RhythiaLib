namespace RhythiaLib.Rhm
{
    public sealed class InvalidRhmFormatException : Exception
    {
        public InvalidRhmFormatException(string message)
            : base(message) { }

        public InvalidRhmFormatException(string message, Exception exception)
            : base(message, exception) { }
    }
}
