using Endpoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IDashboardService
    {
        Task<RadialChartBarDTO> GetTotalBookingsRadialChart();
        Task<RadialChartBarDTO> GetTotalUserRadialChart();
        Task<RadialChartBarDTO> GetTotalRevenueRadialChart();
        Task<PieChartDTO> GetTotalBookingsPieChart();
        Task<LineChartDTO> GetMemberAndLineChartData();

    }
}
