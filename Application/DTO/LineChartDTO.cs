using System.Security.Permissions;

namespace Endpoint.ViewModels
{
    public class LineChartDTO
    {
        public List<ChartDate> Series { get; set; }  
        public string[] Categories { get; set; }

    }


    public class ChartDate()
    {
        public string Name { get; set; }

        public int[] Data { get; set; }
    }
}

