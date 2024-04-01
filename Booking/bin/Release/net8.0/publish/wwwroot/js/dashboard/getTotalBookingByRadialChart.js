
$(document).ready(function () {
	loadBookingRadialChart()
})



function loadBookingRadialChart() {

	$('.chart-spinner').show()

	$.ajax({
		"url": '/admin/GetTotalBookingsRadialChart',
		"type": 'GET',
		dataType: 'json',
		success: function (data) {

			document.querySelector('#spanTotalBookingCount').innerHTML = data.totalCount;

			var increaseSection = document.createElement('span')

			if (data.hasRatioIncreased) {
				increaseSection.className = 'text-success me-1';
				increaseSection.innerHTML = `<i class="bi bi-arrow-up-right-circle me-1"></i> <span>  ${data.countByCurrentMonth} </span>`

			}
			else {
				increaseSection.className = 'text-danger me-1';
				increaseSection.innerHTML = `<i class="bi bi-arrow-down-right-circle me-1"></i> <span>  ${data.countByCurrentMonth} </span>`
			}


			
		

			loadRadialChart('totalBookingsRadialChart', data,'green');


			$('.chart-spinner').hide()

		}
	})

}


