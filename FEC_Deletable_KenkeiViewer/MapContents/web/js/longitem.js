
function clearLine() {
	lineGroup.clearLayers();
}

function drawLine(srcLat, srcLon, dstLat, dstLon, color, weight) {
	var LatLngs = L.polyline([
		[srcLat, srcLon],
		[dstLat, dstLon],
	],{
		"color": color,
		"weight": weight,
		//"opacity": 0.6
	}).addTo(lineGroup);
}
