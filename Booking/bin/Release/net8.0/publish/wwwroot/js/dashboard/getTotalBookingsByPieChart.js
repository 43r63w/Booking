
$(document).ready(function () {

	loadBookingPieChart()

})



function loadBookingPieChart() {
	$('.chart-spinner').show()

	$.ajax({
		"url": '/admin/GetTotalBookingsPieChart',
		"type": 'GET',
		dataType: 'json',
		success: function (data) {
			loadPieChart('customerBookingsPieChart', data);
			$('.chart-spinner').hide()

		}
	})

}




function loadPieChart(id, data) {
	let chartColor = getColorsChartArrays(id)

	options = {
		colors: chartColor,
		series: data.series,
		labels: data.labels,
		chart: {
			type: 'pie',
			width: 350
		},
		stroke: {
			show: false
		},
		legend: {
			position: 'bottom',
			horizontalAlign: 'center',
			labels: {
				colors: '#fff',
				useSeriesColors: true
			}
		}

	}
	var chart = new ApexCharts(document.querySelector("#" + id), options);
	chart.render();
}
