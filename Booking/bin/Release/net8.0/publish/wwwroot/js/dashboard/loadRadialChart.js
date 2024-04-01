function loadRadialChart(id, data, color) {

	let chartColor = getColorsChartArrays(id)

	var options = {
		fill: {
			colors:chartColor
		},
		chart: {
			height: 90,
			width: 90,
			type: "radialBar",
			sparkline: {
				enabled: true
			},
			offsetY: -10
		},
		series: data.series,
		plotOptions: {
			radialBar: {
				dataLabels: {
					value: {
						offsetY: -10,	
						color:color
					}
				}
			}
		},

		stroke: {
			lineCap: "round",
		},
		labels: [""]
	};


	var chart = new ApexCharts(document.querySelector("#" + id), options);

	chart.render();

}



function getColorsChartArrays(id) {

	if (document.getElementById(id) !== null) {

		let colors = document.getElementById(id).getAttribute('data-colors')

		if (colors)
		{
			colors = JSON.parse(colors)

			return colors.map(function (value) {

				let newValue = value.replace('', '')

				if (newValue.indexOf(',') == -1) {
					let color = getComputedStyle(document.documentElement).getPropertyValue(newValue)
					if (color)
					return color
					else 
					return newValue
				}

			})

			

		}


	}


}