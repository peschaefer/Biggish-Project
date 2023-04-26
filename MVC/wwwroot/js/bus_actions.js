var busIdToManage;

function submitNewBus() {
    var formData = $("#createBusForm").serialize();

    $.ajax({
        type: "POST",
        url: "/Bus/Create",
        data: formData,
        success: function (response) {
            var createBusModal = new bootstrap.Modal(document.getElementById('createBusModal'));
            createBusModal.hide();
            location.reload();
        },
        error: function (response) {
            console.error("Error while creating the bus:", response);
        },
    });
}

function showDeleteBusModal(busId) {
    busIdToManage = busId;
    var deleteBusModal = new bootstrap.Modal(document.getElementById('deleteBusModal'));
    deleteBusModal.show();
}

function deleteBus() {
    console.log("BUS ID TO BE DELETED: " + busIdToManage);
    $.ajax({
        type: "POST",
        url: "/Bus/Delete",
        data: { id: busIdToManage },
        success: function (response) {
            var deleteBusModal = new bootstrap.Modal(document.getElementById('deleteBusModal'));
            deleteBusModal.hide();
            location.reload();
        },
        error: function (response) {
            console.error("Error while deleting the bus:", response);
        },
    });
}


