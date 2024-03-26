var dataTable;

$(document).ready(function () {
	loadBookingsTable();
})


function loadBookingsTable() {


	dataTable = $('#bookingTable').DataTable({
		"ajax": { url: "/booking/getall/" },
		"columns": [
			{ "data": "id", "width": "5%" },
			{ "data": "user.email", "width": "5%" },
			{ "data": "totalCost", "width": "5%" },
			{ "data": "villa.name", "width": "5%" },
			{ "data": "status", "width": "5%" },
			{
				"data": "id",
				"render": function (data) {
					return `<div class="w-75 btn-group" role="group">
                     <a href="/booking/getBooking?villaId=${data}" class="btn btn-warning mx-2"> <i class="bi bi-pencil-square"></i></a>                                  
                    </div>`
				},
				"width": "10%"
			}
		]



	})

}