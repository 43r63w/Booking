var dataTable


$(document).ready(function () {
	loadDataTable()
})


function loadDataTable() {
	dataTable = $('#villaTable').DataTable({
		"ajax": { url: '/villa/getAll' },
		"columns": [
			{ "data": "id", "width": "2%" },
			{ "data": "name", "width": "15%" },
			{ "data": "price", "width": "10%" },
			{ "data": "sqft", "width": "5%" },
			{ "data": "occupancy", "width": "5%" },
			{
				data: 'id',
				"render": function (data) {
					return `<div class="w-75 btn-group" role="group">
                     <a href="/villa/upsert?villaId=${data}" class="btn btn-success mx-2"> <i class="bi bi-pencil-square"></i></a>               
                     <a onClick=Remove('/villa/remove?villaId=${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i></a>
                    </div>`
				},
				"width": "20%"
			}


		]

	});
}




function Remove(url) {
	$.ajax({
		url: url,
		type: 'DELETE',
		success: function (data) {
			dataTable.ajax.reload();
			toastr.success.message(data.message)
		}
	})
}