
var dataTable



$(document).ready(() => {
	loadAmenityDataTable()
})


function loadAmenityDataTable() {
	dataTable = $('#amenityTable').DataTable({
		"ajax": { url: "/villaAmenity/getAll" },
		"columns": [
			{ "data": "id", "width": "10%" },
			{ "data": "name", "width": "10%" },
			{ "data": "villa.name", "width": "10%" },
			{

				data: "id",

				"render": (data) => {
					return `<div class="w-75 btn-group" role="group">
                     <a href="/villaAmenity/upsert?amenityId=${data}" class="btn btn-success mx-2"> <i class="bi bi-pencil-square"></i></a>
                     <a onClick=Remove('/villaAmenity/remove?amenityId=${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i></a>
                    </div>`
				},
				"width": "25%"
			},
		]
	})
}


function Remove(url) {
	$.ajax({
		url: url,
		type: 'DELETE',
		success: (data) => {
			dataTable.ajax.reload();
			toastr.success.message(data.message)
		}
	})
}