function clearCircle(){
	circleGroup.clearLayers();
}

function drawCircle(latitude, longitude, color, radius){
    var circle = L.circle([latitude, longitude], {
        radius: radius,
        color: color,
        fillColor: color,
        fillOpacity: 0.2,
		weight: 1
    }).addTo(circleGroup);
}
