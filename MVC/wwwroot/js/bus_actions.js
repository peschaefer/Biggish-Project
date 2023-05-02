var busIdToDelete;
var selectedBusIds;

$(document).ready(function () {
    // Handle "select all" checkbox
    $("#selectAllCheckbox").on("change", function () {
        $('input[name="busCheckbox"]').prop("checked", this.checked);
    });
});

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

function showDeleteBusModal() {
    selectedBusIds = $('input[name="busCheckbox"]:checked').map(function () {
        return parseInt($(this).data('bus-id'));
    }).get();

    var deleteBusModal = new bootstrap.Modal(document.getElementById('deleteBusModal'));
    deleteBusModal.show();
}

function deleteSelectedBuses() {
    $.ajax({
        type: "POST",
        url: "/Bus/Delete",
        data: { ids: selectedBusIds },
        traditional: true,
        success: function (response) {
            $("#selectAllCheckbox").prop('checked', false);
            $('input[name="busCheckbox"]').prop('checked', false);
            var deleteBusModal = new bootstrap.Modal(document.getElementById('deleteBusModal'));
            deleteBusModal.hide();
            location.reload();
        },
        error: function (response) {
            console.error("Error while deleting the buses:", response);
        },
    });
}    

function showEditBusModal(busId) {
    console.log("BUSiD" + busId)
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
