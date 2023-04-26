var busIdToDelete;

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
    busIdToDelete = busId;
    var deleteBusModal = new bootstrap.Modal(document.getElementById('deleteBusModal'));
    deleteBusModal.show();
}

function deleteBus() {
    $.ajax({
        type: "POST",
        url: "/Bus/Delete",
        data: { id: busIdToDelete },
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

function showEditBusModal(busId) {
    document.getElementById('editBusId').value = busId;
    var editBusModal = new bootstrap.Modal(document.getElementById('editBusModal'));
    editBusModal.show();
}

function editBus() {
    var formData = $("#editBusForm").serialize();
    console.log(formData);
    
    $.ajax({
        type: "POST",
        url: "/Bus/Edit",
        data: formData,
        success: function (response) {
            var editBusModal = new bootstrap.Modal(document.getElementById('editBusModal'));
            editBusModal.hide();
            location.reload();
        },
        error: function (response) {
            console.error("Error while editing the bus:", response.responseText);
        },
    });
}
