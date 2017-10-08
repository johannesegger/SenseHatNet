namespace SenseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = args.Length > 0 ? args[0] : "Hello World";
            Sense.Led.LedMatrix.ShowMessage(text);
        }
    }
}
