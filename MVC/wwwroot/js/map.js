
let map;

function initMap() {
  const position = { lat: -25.344, lng: 131.031 };

  map = new google.maps.Map(document.getElementById("map"), {
    zoom: 4,
    center: position,
  });

  const marker = new google.maps.Marker({
    map: map,
    position: position,
    title: "Uluru",
  });
}

window.addEventListener("load", initMap);


