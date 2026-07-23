namespace RhythiaLib.Rhr
{
    public sealed class InvalidRhrFormatException : Exception
    {
        public InvalidRhrFormatException(string message)
            : base(message) { }

        public InvalidRhrFormatException(string message, Exception exception)
            : base(message, exception) { }
    }
}
