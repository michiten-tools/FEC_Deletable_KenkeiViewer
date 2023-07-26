// JavaScript source code

// Circle
function clearCircle(){
	circleGroup.clearLayers();
}

// Circle
function drawCircle(latitude, longitude, color, radius){
    var circle = L.circle([latitude, longitude], {
        radius: radius,
        color: color,
        fillColor: color,
        fillOpacity: 0.2,
		weight: 1
    }).addTo(circleGroup);
}

function moveMap(latitude, longitude) {
    mymap.panTo(new L.LatLng(latitude, longitude));
}

function closePopup(){
    mymap.closePopup();
}

function openPopup(lat, lng){

    var visibleMarkers = [];
    this.mymap.eachLayer(function (layer) {
        if (layer instanceof L.Marker) {
            visibleMarkers.push(layer);
        }
    });

    for (var i = 0; i < visibleMarkers.length; i++) {
        var latlng = visibleMarkers[i].getLatLng();
        if (latlng.lat == lat && latlng.lng == lng){
            visibleMarkers[i].openPopup();
            break;
        }
    }
}

// 未使用 ポップアップをクリックするとC#側の関数を呼び出す
function onPopupClick(id, serial) {

}

function onMapClick(e) {

    popup
        .setLatLng(e.latlng)
        .setContent("緯度：" + e.latlng.lat.toString() + "<br>経度：" + e.latlng.lng.toString())
        .openOn(mymap);
}

function onManualClick(e) {
    clearTypeDst();

}

function setZoomMain() {
    mymap.setZoom(18);
}


function getMapBounds() {
    var bounds = mymap.getBounds();
    var northWest = bounds.getNorthWest(),
        northEast = bounds.getNorthEast(),
        southWest = bounds.getSouthWest(),
        southEast = bounds.getSouthEast();

    chrome.webview.hostObjects.class.GetMapBounds(
        northWest.lat + "," + northWest.lng + "," +
        northEast.lat + "," + northEast.lng + "," +
        southWest.lat + "," + southWest.lng + "," +
        southEast.lat + "," + southEast.lng);
}

function SetMaxBounds() {
    mymap.setMaxBounds(bounds_all);
}

// 日本の領域だけ表示
var bounds_all = [
    [20.0000, 122.0000],
    [46.0000, 154.0000]
];

function setCaptureZoom() {
    mymap.setZoom(17);
}

function setZoomManual(i) {
    mymap.setZoom(i);
}