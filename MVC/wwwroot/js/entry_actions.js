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
