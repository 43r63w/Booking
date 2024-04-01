namespace Endpoint.ViewModels
{
    public class RadialChartBarDTO
    {

        public decimal TotalCount { get; set; }
        public int CountByCurrentMonth { get; set; }
        public bool HasRatioIncreased { get; set; }
        public int[] Series { get; set; }
        
    }
}
