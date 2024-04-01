$(document).ready(function () {
	loadUserRadialChart()
})



function loadUserRadialChart() {

	$('.chart-spinner').show()

	$.ajax({
		url: '/admin/GetTotalUserRadialChart',
		type: 'GET',
		dataType: 'json',
		success: function (data) {
			document.querySelector('#spanTotalUserCount').innerHTML = data.totalCount

			let createSpan = document.createElement('span')

			if (data.hasRatioIncreased) {
				createSpan.className = `text-success me-1`
				createSpan.innerHTML = `<i class="bi bi-arrow-up-right-circle me-1"></i> <span>  ${data.countByCurrentMonth} </span>`
			}
			else {
				createSpan.className = `text-danger me-1`
				createSpan.innerHTML = `<i class="bi bi-arrow-down-right-circle me-1"></i> <span>  ${data.countByCurrentMonth} </span>`
			}

			document.querySelector('#sectionUserCount').append(createSpan)
			document.querySelector('#sectionUserCount').append('since last month')

			loadRadialChart('totalUserRadialChart', data,'yellow');

			$('.chart-spinner').hide()
		}
	})
}




