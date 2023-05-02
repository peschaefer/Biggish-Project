var stopIdToDelete;
var selectedStopIds;

$(document).ready(function () {
    // Handle "select all" checkbox
    $("#selectAllCheckbox").on("change", function () {
        $('input[name="stopCheckbox"]').prop("checked", this.checked);
    });
});

function submitNewStop() {
    var formData = $("#createStopForm").serialize();

    $.ajax({
        type: "POST",
        url: "/Stop/Create",
        data: formData,
        success: function (response) {
            var createStopModal = new bootstrap.Modal(document.getElementById('createStopModal'));
            createStopModal.hide();
            location.reload();
        },
        error: function (response) {
            console.error("Error while creating the stop:", response);
        },
    });
}

function showDeleteStopModal() {
    selectedStopIds = $('input[name="stopCheckbox"]:checked').map(function () {
        return parseInt($(this).data('stop-id'));
    }).get();

    var deleteStopModal = new bootstrap.Modal(document.getElementById('deleteStopModal'));
    deleteStopModal.show();
}

function deleteSelectedStops() {
    $.ajax({
        type: "POST",
        url: "/Stop/Delete",
        data: { ids: selectedStopIds },
        traditional: true,
        success: function (response) {
            $("#selectAllCheckbox").prop('checked', false);
            $('input[name="stopCheckbox"]').prop('checked', false);
            var deleteStopModal = new bootstrap.Modal(document.getElementById('deleteStopModal'));
            deleteStopModal.hide();
            location.reload();
        },
        error: function (response) {
            console.error("Error while deleting the stopes:", response);
        },
    });
}

function showEditStopModal(stopId) {
    document.getElementById('editStopId').value = stopId;
    var editStopModal = new bootstrap.Modal(document.getElementById('editStopModal'));
    editStopModal.show();
}

function editStop() {
    var formData = $("#editStopForm").serialize();
    console.log(formData);

    $.ajax({
        type: "POST",
        url: "/Stop/Edit",
        data: formData,
        success: function (response) {
            var editStopModal = new bootstrap.Modal(document.getElementById('editStopModal'));
            editStopModal.hide();
            location.reload();
        },
        error: function (response) {
            console.error("Error while editing the stop:", response.responseText);
        },
    });
}
