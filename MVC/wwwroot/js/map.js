let map;
let markers = [];

function initMap() {
    // The location of Bracken Library
    const position = {lat: 40.20252042888084, lng: -85.40723289777657};

    map = new google.maps.Map(document.getElementById("map"), {
        zoom: 15,
        center: position,
    });


    const stops = window.stopsData;


    let mostCrowdedStop = {passengers: 0};
    console.log(stops)

    stops.forEach(stop => {
        if (stop.passengers > mostCrowdedStop.passengers) {
            mostCrowdedStop = stop;
        }
    });

    stops.forEach(stop => {
        const stopPosition = {lat: stop.latitude, lng: stop.longitude};
        const isMostCrowded = stop.id === mostCrowdedStop.id;
        if (isMostCrowded) console.log(stop)

        const marker = new google.maps.Marker({
            map: map,
            position: stopPosition,
            title: `${stop.name} (${stop.passengers} passengers)`,
            icon: isMostCrowded ? 'http://maps.google.com/mapfiles/ms/icons/red-dot.png' : 'http://maps.google.com/mapfiles/ms/icons/blue-dot.png'
        });

        markers.push(marker);
    });

    updateMapMarkers(window.stopsData);
}

function updateMapMarkers(stops) {
    // Clear existing markers
    markers.forEach(marker => marker.setMap(null));
    markers = [];

    let mostCrowdedStop = {passengers: 0};

    stops.forEach(stop => {
        if (stop.passengers > mostCrowdedStop.passengers) {
            mostCrowdedStop = stop;
        }
    });

    // Add new markers
    stops.forEach(stop => {
        const stopPosition = {lat: stop.latitude, lng: stop.longitude};
        const isMostCrowded = stop.id === mostCrowdedStop.id;
        const marker = new google.maps.Marker({
            map: map,
            position: stopPosition,
            title: `${stop.name} (${stop.passengers} passengers)`,
            icon: isMostCrowded ? 'http://maps.google.com/mapfiles/ms/icons/red-dot.png' : 'http://maps.google.com/mapfiles/ms/icons/blue-dot.png'
        });

        markers.push(marker);
    });
}

window.addEventListener("load", initMap);
