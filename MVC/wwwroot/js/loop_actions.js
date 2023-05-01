var loopIdToDelete;
var selectedLoopIds;

$(document).ready(function () {
    // Handle "select all" checkbox
    $("#selectAllCheckbox").on("change", function () {
        $('input[name="loopCheckbox"]').prop("checked", this.checked);
    });
});

function submitNewLoop() {
    console.log("submitNewLoop")
    var formData = $("#createLoopForm").serialize();

    $.ajax({
        type: "POST",
        url: "/Loop/Create",
        data: formData,
        success: function (response) {
            var createLoopModal = new bootstrap.Modal(document.getElementById('createLoopModal'));
            createLoopModal.hide();
            location.reload();
        },
        error: function (response) {
            console.error("Error while creating the loop:", response);
        },
    });
}

function showDeleteLoopModal() {
    selectedLoopIds = $('input[name="loopCheckbox"]:checked').map(function () {
        return parseInt($(this).data('loop-id'));
    }).get();

    var deleteLoopModal = new bootstrap.Modal(document.getElementById('deleteLoopModal'));
    deleteLoopModal.show();
}

function deleteSelectedLoops() {
    $.ajax({
        type: "POST",
        url: "/Loop/Delete",
        data: { ids: selectedLoopIds },
        traditional: true,
        success: function (response) {
            $("#selectAllCheckbox").prop('checked', false);
            $('input[name="loopCheckbox"]').prop('checked', false);
            var deleteLoopModal = new bootstrap.Modal(document.getElementById('deleteLoopModal'));
            deleteLoopModal.hide();
            location.reload();
        },
        error: function (response) {
            console.error("Error while deleting the loops:", response);
        },
    });
}    

function showEditLoopModal(loopId) {
    document.getElementById('editLoopId').value = loopId;
    var editLoopModal = new bootstrap.Modal(document.getElementById('editLoopModal'));
    editLoopModal.show();
}

function editLoop() {
    var formData = $("#editLoopForm").serialize();
    console.log(formData);
    
    $.ajax({
        type: "POST",
        url: "/Loop/Edit",
        data: formData,
        success: function (response) {
            var editLoopModal = new bootstrap.Modal(document.getElementById('editLoopModal'));
            editLoopModal.hide();
            location.reload();
        },
        error: function (response) {
            console.error("Error while editing the loop:", response.responseText);
        },
    });
}



