namespace Runtime.Popup
{
    public sealed class PopupData
    {
        public string Title { get; }
        public string Message { get; }

        public PopupData(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}