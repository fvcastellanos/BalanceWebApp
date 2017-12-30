
namespace BalanceWebApp.Model.Views
{
    public class Option
    {
        public string Value { get; set; }
        public string Text { get; set; }

        public Option()
        {
        }

        public Option(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}