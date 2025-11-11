namespace inclassLottery;

class Ticket
{
    public int[] RegTickets { get; set; }
    public int PowerBall { get; set; }
    public Ticket(int[] numbers, int powerBall)
    {
        RegTickets = new int[5];
        RegTickets[0] = numbers[0];
        RegTickets[1] = numbers[1];
        RegTickets[2] = numbers[2];
        RegTickets[3] = numbers[3];
        RegTickets[4] = numbers[4];
        PowerBall = powerBall;
    }
    public Ticket()
    {
        RegTickets = new int[5];
        for (int i = 0; i < 5; i++)
        {
            RegTickets[i] = Random.Shared.Next(1, 70);
        }
        PowerBall = Random.Shared.Next(1, 27);
    }
    public int Judgeifwin(Ticket winningTicket)
    {
        int winconitionsmet = 0;
        for (int i = 0; i < 5; i++)
        {
            if (RegTickets[i] == winningTicket.RegTickets[i])
            {
                winconitionsmet++;
            }
        }
        if (PowerBall == winningTicket.PowerBall)
        {
            winconitionsmet++;
        }
        return winconitionsmet;
    }
}
class LotteryPeriod
{
    public Ticket WinningTicket { get; set; }
    public List<Ticket> SoldTickets { get; set; } = new List<Ticket>();
    public LotteryPeriod()
    {
        int[] numbers = new int[5] { 1, 2, 3, 4, 5 };
        SetWinningTicket(numbers, 6);

    }
    public void SetWinningTicket(int[] numbers, int powerBall)
    {
        WinningTicket = new Ticket(numbers, powerBall);
    }
    public void getherStatistics()
    {
        Dictionary<int, int> statistics = new Dictionary<int, int>();
        int Win = 0;
        int numbers = 0;
        for (int i = 0; i <= 69; i++)
        {
            statistics[i] = 0;
        }
         foreach (Ticket ticket in SoldTickets)
        {
            Win = ticket.Judgeifwin(WinningTicket);
            statistics[Win]++;
        }
        Console.WriteLine("Winning Statistics:");
        for (int i = 0; i <= 4; i++)
        {
            Console.WriteLine($"winning ticket number {WinningTicket.RegTickets[i]} has {statistics[i]} matches");
        }
        for (int i = 5; i < 6; i++)
        {
            Console.WriteLine($"winning PowerBall number {WinningTicket.PowerBall} has {statistics[i]} matches");
        }

    }
}
class LotteryVendor
{
    public static readonly object lockObject = new object();
    public LotteryVendor()
    {
    }
    public void SellTickets(LotteryPeriod period, int numberOfTickets)
    {
        for (int i = 0; i < numberOfTickets; i++)
        {
            lock(lockObject)
            {
            Ticket ticket = new Ticket();
            period.SoldTickets.Add(ticket);
            }
        }
    }
}
class Program
{

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Lets sell 1Million Tickets!");
        LotteryPeriod period = new LotteryPeriod();
        List<LotteryVendor> vendors = new List<LotteryVendor>();
        for (int i = 0; i < 3; i++)
        {
            vendors.Add(new LotteryVendor());
        }
        Parallel.ForEach(vendors, vendor =>
        {
            vendor.SellTickets(period, 1_000_000);
        });
        Console.WriteLine("SOLD 30Million Tickets!");
        period.getherStatistics();

        //TODO: 1a) make 3 vendors sell 10M tickets each good
        // 1b) 3 vendors sell tickets in parallel good
        // 2) Modify Ticket class to be able to judge a winner level good
        // 3) Gather statistics on how many winners of each level there are
        // 4) Print out the statistics
        // AFTER 1-4 is working, try to do (GatherStatistics) with Parallel Programming
    }
}
