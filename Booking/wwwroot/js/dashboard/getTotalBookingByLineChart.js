
$(document).ready(function () {
	loadMemberAndBookingsLineChart()
})


function loadMemberAndBookingsLineChart() {

	$('.chart-spinner').show()

	$.ajax({
		"url": '/admin/GetMemberAndLineChartData',
		"type": 'GET',
		dataType: 'json',
		success: function (data) {
			loadLineChart('getMemberAndBookingDataLineChart', data);
			$('.chart-spinner').hide()

		}
	})
}

function loadLineChart(id, data) {

	let chartColor = getColorsChartArrays(id)

	var options = {

		colors: chartColor,
		series: data.series,
		chart: {
			height: 350,
			type: 'line',
		},
		stroke: {
			curve: 'smooth',
			width:2
		},
		markers: {
			size: 5,
			strokeWidth:0,
			hover: {
				size:11,
			}
		},
		xaxis: {
			categories: data.categories,
			labels: {
				style: {
					colors: '#fff'
				}
			}
		},
		yaxis: {
			labels: {
				style: {
					colors:'#fff'
				}
			}
		},
		legend: {
			labels: {
				colors: '#fff'	
			}
		},
		tooltip: {
			theme:'dark'
		}
	}
	var chart = new ApexCharts(document.querySelector("#" + id), options);

	chart.render();
}