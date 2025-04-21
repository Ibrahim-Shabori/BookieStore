$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else if (url.includes("completed")) {
        loadDataTable("completed");
    }
    else if (url.includes("pending")) {
        loadDataTable("pending");
    }
    else if (url.includes("approved")) {
        loadDataTable("approved");
    }
    else {
        loadDataTable("all");
    }
});

var dataTable;
function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        'ajax': {
            url: '/Admin/Order/GetAll?status=' + status,
            // No need for dataSrc modification as response already has data property
        },
        'columns': [
            { data: "id", width: "5%" },
            { data: "name", width: "15%" },
            { data: "phoneNumber", width: "15%" },
            { data: "applicationUser.email", width: "25%" },
            { data: "orderStatus", width: "10%" },
            { data: "orderTotal", width: "15%" },
            {
                // Action column for the empty header
                data: 'id', width: "15%",
                'render': function (data) {
                return `
                <div class="btn-group w-100" role="group">
                    <a href="/admin/order/details?orderId=${data}" class="btn btn-outline-primary mx-2">
                        <i class="bi bi-pencil-square"></i>
                    </a>
                </div>
                `}
            }
        ]
    });
}
