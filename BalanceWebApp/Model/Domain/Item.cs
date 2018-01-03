namespace BalanceWebApp.Model.Domain
{
    public class Item
    {
        public string Value { get; }
        public string Text { get; }

        public Item(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}