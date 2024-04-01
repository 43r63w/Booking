var dataTable


$(document).ready(function () {
	loadDataTable()
})


function loadDataTable() {
	dataTable = $('#villaRoomNumber').DataTable({
		"ajax": { url: '/villaNumber/getAll' },
		"columns": [
			{ "data": "villa_Number", "width": "5%" },
			{ "data": "villa.name", "width": "15%" },
			{ "data": "specialDetails", "width": "15%" },
			{
				data: 'villa_Number',
				"render": function (data) {
					return `<div class="w-75 btn-group" role="group">
                     <a href="/villaNumber/upsert?villaNumberId=${data}" class="btn btn-success mx-2"> <i class="bi bi-pencil-square"></i></a>
                     <a onClick=Remove('/villaNumber/remove?villaNumberId=${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i></a>
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
		type: "DELETE",
		success: (data) => {
			dataTable.ajax.reload(),
			toastr.success.message(data.message)
		}
	})

}