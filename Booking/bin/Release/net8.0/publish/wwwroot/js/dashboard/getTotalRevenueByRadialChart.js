$(document).ready(function () {
	loadRevenueRadialChart()
})





function loadRevenueRadialChart() {


	$('.chart-spinner').show()


	$.ajax({
		"url": '/admin/GetTotalRevenueChart',
		"type": 'GET',
		dataType: 'json',
		success: function (data) {


			document.querySelector('#spanTotalRevenueCount').innerHTML = data.totalCount


	      	var increaseSection = document.createElement('span')
			if (data.hasRatioIncreased) {
				increaseSection.className = 'text-success me-1';
				increaseSection.innerHTML = `<i class="bi bi-arrow-up-right-circle me-1"></i> <span>  ${data.countByCurrentMonth} </span>`
			}
			else {
				increaseSection.className = 'text-danger me-1';
				increaseSection.innerHTML = `<i class="bi bi-arrow-down-right-circle me-1"></i> <span>  ${data.countByCurrentMonth} </span>`
			}

			document.querySelector('#sectionRevenueCount').append(increaseSection)
			document.querySelector('#sectionRevenueCount').append('since last month')

			loadRadialChart('totalRevenueRadialChart', data,'#1f9eb6')

			$('.chart-spinner').hide()
		}
	})



}