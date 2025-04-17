$(document).ready(function () {
    LoadDataTable();
});

var dataTable;
function LoadDataTable() {
    dataTable = $('#tblData').DataTable({
        'ajax': { 
            url: '/Admin/Product/GetAll'
            // No need for dataSrc modification as response already has data property
        },
        'columns': [
            { data: "title", width: "25%" },
            { data: "isbn", width: "15%" },
            { data: "author", width: "10%" },
            { data: "category.name", width: "15%" },
            { data: "listPrice", width: "10%" },
            {
                // Action column for the empty header
                data: 'id', width: "25%",
                'render': function (data) {
                return `
                <div class="btn-group w-100" role="group">
                    <a href="/admin/product/upsert?id=${data}" class="btn btn-outline-primary mx-2">
                        <i class="bi bi-pencil-square"></i> Edit
                    </a>
                    <a onClick=Delete("/admin/product/delete?id=${data}") class="btn btn-outline-danger mx-2">
                        <i class="bi bi-trash-fill"></i> Delete
                    </a>
                </div>
                `}
                // Or leave empty if it's just a placeholder
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