namespace TimeSpanSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TimeSpan timeStart = new TimeSpan(1000);
            // 10_000_00 = 1000 = 1;

            // Universal Coordinated Time (UTC)
            timeStart = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            Console.WriteLine(timeStart);

            Random random = new Random();
            Thread.Sleep(random.Next(1500, 3000)); // 1.5 c - 3c


            TimeSpan timeEnd = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            TimeSpan timeDiff = timeEnd - timeStart;

            Console.WriteLine(timeDiff.ToString("dd\\.hh\\:mm\\:ss\\:FFF"));

            Console.ReadKey();

        }
    }
}