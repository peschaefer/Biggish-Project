function showDeleteEntryModal() {
    selectedEntryIds = $('input[name="entryCheckbox"]:checked').map(function () {
        return parseInt($(this).data('entry-id'));
    }).get();

    var deleteEntryModal = new bootstrap.Modal(document.getElementById('deleteEntryModal'));
    deleteEntryModal.show();
}

function deleteSelectedEntries() {
    $.ajax({
        type: "POST",
        url: "/Entry/Delete",
        data: { ids: selectedEntryIds },
        traditional: true,
        success: function (response) {
            $("#selectAllCheckbox").prop('checked', false);
            $('input[name="entryCheckbox"]').prop('checked', false);
            var deleteEntryModal = new bootstrap.Modal(document.getElementById('deleteEntryModal'));
            deleteEntryModal.hide();
            location.reload();
        },
        error: function (response) {
            console.error("Error while deleting the entries:", response);
        },
    });
}    

function applyFilterAndSort() {
    const busFilters = Array.from(document.getElementsByName("busFilter"))
        .filter(checkbox => checkbox.checked)
        .map(checkbox => parseInt(checkbox.value));
    const driverFilters = Array.from(document.getElementsByName("driverFilter"))
        .filter(checkbox => checkbox.checked)
        .map(checkbox => checkbox.value);
    const loopFilters = Array.from(document.getElementsByName("loopFilter"))
        .filter(checkbox => checkbox.checked)
        .map(checkbox => checkbox.value);
    const stopFilters = Array.from(document.getElementsByName("stopFilter"))
        .filter(checkbox => checkbox.checked)
        .map(checkbox => checkbox.value);

    const sortBy = document.getElementById("sortBy").value;
    
    const tableRows = document.querySelectorAll("#entry-table tbody tr");

    // Show all rows
    tableRows.forEach(row => {
        row.style.display = "";
    });

    // Apply the filters
    tableRows.forEach(row => {
        const bus = parseInt(row.querySelector('td[data-attribute="bus"]').textContent);
        const driver = row.querySelector('td[data-attribute="driver"]').textContent;
        const loop = row.querySelector('td[data-attribute="loop"]').textContent;
        const stop = row.querySelector('td[data-attribute="stop"]').textContent;

        if ((busFilters.length > 0 && !busFilters.includes(bus)) ||
            (driverFilters.length > 0 && !driverFilters.includes(driver)) || 
            (loopFilters.length > 0 && !loopFilters.includes(loop)) ||
            (stopFilters.length > 0 && !stopFilters.includes(stop))) {
            row.style.display = "none";
        }
    });

    // Apply the sort
    if (sortBy) {
        const sortOrder = document.getElementById("sortOrder").value;
    
        const sortedRows = Array.from(tableRows)
            .sort((a, b) => {
                const aVal = a.querySelector(`td[data-attribute="${sortBy}"]`).textContent;
                const bVal = b.querySelector(`td[data-attribute="${sortBy}"]`).textContent;
                console.log(new Date(aVal))
                let compareResult;
                if (sortBy === "timestamp") {
                    compareResult = new Date(aVal) - new Date(bVal);
                } else {
                    compareResult = aVal.localeCompare(bVal);
                }
    
                return sortOrder === "ascending" ? compareResult : -compareResult;
            });
    
        const tbody = document.querySelector("#entry-table tbody");
        sortedRows.forEach(row => tbody.appendChild(row));
    }
}

function tableToCSV() {
    const table = document.getElementById("entry-table");
    const headers = Array.from(table.querySelectorAll("th"))
        .slice(1, -1)
        .map(header => header.textContent.trim())
        .join(",");

    const rows = Array.from(table.querySelectorAll("tbody tr"))
        .filter(row => row.style.display !== "none")
        .map(row =>
            Array.from(row.querySelectorAll("td"))
                .slice(1, -1)
                .map(cell => cell.textContent.trim())
                .join(",")
        )
        .join("\n");

    return `${headers}\n${rows}`;
}

function downloadCSV(csvContent, fileName) {
    const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
    const link = document.createElement("a");

    const url = URL.createObjectURL(blob);
    link.setAttribute("href", url);
    link.setAttribute("download", fileName);
    link.style.visibility = "hidden";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
