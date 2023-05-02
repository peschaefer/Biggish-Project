let map;
let markers = [];

function initMap() {
    // The location of Bracken Library
    const position = { lat: 40.20252042888084, lng: -85.40723289777657 };

    map = new google.maps.Map(document.getElementById("map"), {
        zoom: 15,
        center: position,
    });

    const stops = window.stopsData;

    stops.forEach(stop => {
        const stopPosition = { lat: stop.latitude, lng: stop.longitude };
        const marker = new google.maps.Marker({
            map: map,
            position: stopPosition,
            title: stop.name,
        });

        markers.push(marker);
    });

    updateMapMarkers(window.stopsData);
}

function updateMapMarkers(stops) {
    // Clear existing markers
    markers.forEach(marker => marker.setMap(null));
    markers = [];

    // Add new markers
    stops.forEach(stop => {
        const stopPosition = { lat: stop.latitude, lng: stop.longitude };
        const marker = new google.maps.Marker({
            map: map,
            position: stopPosition,
            title: stop.name,
        });

        markers.push(marker);
    });
}

window.addEventListener("load", initMap);