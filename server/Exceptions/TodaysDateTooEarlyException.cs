namespace InboxEngine.Exceptions
{
    [Serializable]
    public class TodaysDateTooEarlyException : Exception
    {
        public TodaysDateTooEarlyException() : base() { }
        public TodaysDateTooEarlyException(string message) : base(message) { }
        public TodaysDateTooEarlyException(string message, Exception inner) : base(message, inner) { }
    }

}
