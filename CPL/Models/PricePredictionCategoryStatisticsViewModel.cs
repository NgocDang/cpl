namespace CPL.Models
{
    public class PricePredictionCategoryStatisticsViewModel
    {
        public int TotalRevenue { get; set; }
        public int TotalSale { get; set; }
        public int PageView { get; set; }
        public int TotalPlayers { get; set; }
        public int TodayPlayers { get; set; }

        public string TotalRevenueChangesInJson { get; set; }
        public string TotalSaleChangesInJson { get; set; }
        public string PageViewChangesInJson { get; set; }
        public string TotalPlayersChangesInJson { get; set; }
    }
}
