



var data = [
    [139.770392, 35.684735],
    [139.771051, 35.684418],
    [139.770843, 35.683849],
    [139.770150, 35.684007]
];


function addPolygon(latlng, color) {

    let latlngAry = latlng.split(',');
    let dataAry = new Array();
    for (let i = 0; i < latlngAry.length / 2 - 1; i++) {
        dataAry.push([latlngAry[2 * i], latlngAry[2*i+1] ]);
    }
    
    addPolygonLayer(getGeoJson(dataAry, color));

}


function getGeoJson(coordinates, color) {

    return [{
        "type": "Feature",
        "properties": { "party": color },
        "geometry": {
            "type": "Polygon",
            "coordinates": [ coordinates ]
        }
    }];
}


function addPolygonLayer(geojson) {
    L.geoJSON(geojson, {
        style: function (feature) {
            switch (feature.properties.party) {
                case 'red': return { color: "#ff0000", fillColor: "#000000", fillOpacity: 0.0, dashArray: "10 10", weight: 6 };
                case 'blue': return { color: "#0000ff" };
            }
        }
    }).addTo(polygonLayer);
}


function clearPolygonLayer() {
    polygonLayer.clearLayers();
}