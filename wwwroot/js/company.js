$(document).ready(function () {
    LoadDataTable();
});

var dataTable;
function LoadDataTable() {
    dataTable = $('#tblData').DataTable({
        'ajax': {
            url: '/Admin/Company/GetAll'
            // No need for dataSrc modification as response already has data property
        },
        'columns': [
            { data: "name", width: "10%" },
            { data: "streetAddress", width: "20%" },
            { data: "city", width: "15%" },
            { data: "state", width: "15%" },
            { data: "phoneNumber", width: "15%" },
            {
                // Action column for the empty header
                data: 'id', width: "25%",
                'render': function (data) {
                    return `
                <div class="btn-group w-100" role="group">
                    <a href="/admin/company/upsert?id=${data}" class="btn btn-outline-primary mx-2">
                        <i class="bi bi-pencil-square"></i> Edit
                    </a>
                    <a onClick=Delete("/admin/company/delete?id=${data}") class="btn btn-outline-danger mx-2">
                        <i class="bi bi-trash-fill"></i> Delete
                    </a>
                </div>
                `}
            }
        ]
    });
}


function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                },
                error: function (xhr, status, error) {
                    toastr.error(xhr.responseText);
                }
            });
        }
    });
}