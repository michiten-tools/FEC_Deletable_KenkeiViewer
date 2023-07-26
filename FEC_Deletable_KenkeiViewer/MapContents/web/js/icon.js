// icon ------------------
var iconCursor, iconKml;
var iconA, iconB, iconC, iconD, iconK, iconL, iconO, iconP,
    iconS, iconT, iconU, iconV, iconW, iconX, iconE, iconF,
    iconQ, iconN, iconM;
var iconOrg, iconSrc, iconDst, redIcon;

var iconDir = 'saitama';

function setIconDir(_dir) {
    iconDir = _dir;
    setIcons();
}

function setIcons() {

    // cursor
    var iconCursor = new L.Icon({
        iconUrl: 'markerIcon/cursor6.png',
        iconSize: [32, 32],
        iconAnchor: [16, 16],
        popupAnchor: [1, -24]
    });

    // kml
    var iconKml = new L.Icon({
        iconUrl: 'markerIcon/kml4.png',
        iconSize: [12, 12],
        iconAnchor: [6, 6]
    });

    iconA = new L.Icon({
        iconUrl: iconDir + '/iconA.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });

    iconB = new L.Icon({
        iconUrl: iconDir + '/iconB.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconC = new L.Icon({
        iconUrl: iconDir + '/iconC.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconD = new L.Icon({
        iconUrl: iconDir + '/iconD.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconK = new L.Icon({
        iconUrl: iconDir + '/iconK.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconL = new L.Icon({
        iconUrl: iconDir + '/iconL.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconO = new L.Icon({
        iconUrl: iconDir + '/iconO.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconP = new L.Icon({
        iconUrl: iconDir + '/iconP.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconS = new L.Icon({
        iconUrl: iconDir + '/iconS.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconT = new L.Icon({
        iconUrl: iconDir + '/iconT.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconU = new L.Icon({
        iconUrl: iconDir + '/iconU.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconV = new L.Icon({
        iconUrl: iconDir + '/iconV.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconW = new L.Icon({
        iconUrl: iconDir + '/iconW.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconX = new L.Icon({
        iconUrl: iconDir + '/iconX.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconE = new L.Icon({
        iconUrl: iconDir + '/iconE.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconF = new L.Icon({
        iconUrl: iconDir + '/iconF.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconM = new L.Icon({
        iconUrl: iconDir + '/iconM.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconN = new L.Icon({
        iconUrl: iconDir + '/iconN.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });
    iconQ = new L.Icon({
        iconUrl: iconDir + '/iconQ.png',
        iconSize: [32, 32],
        iconAnchor: [8, 16],
        popupAnchor: [1, -24],
        className: 'icon'
    });

    var iconOrg = new L.Icon({
        iconUrl: 'markerIcon/marker-icon-blue.png',
        iconSize: [12, 20],//[25, 41],
        iconAnchor: [6, 20],//[12, 41],
        popupAnchor: [0, -20]
    });
    var iconSrc = new L.Icon({
        iconUrl: 'markerIcon/marker-icon-green.png',
        iconSize: [12, 20],//[25, 41],
        iconAnchor: [6, 20],//[12, 41],
        popupAnchor: [0, -20]
    });
    var iconDst = new L.Icon({
        iconUrl: 'markerIcon/marker-icon-red.png',
        iconSize: [12, 20],//[25, 41],
        iconAnchor: [6, 20],//[12, 41],
        popupAnchor: [0, -20]
    });
}
// clear ------------------

// cursor
function clearCursor(){
    cursorGroup.clearLayers();
}

// kml
function clearKmls() {
    kmlGroup.clearLayers();
}

// icon
function clearTypeA(){
    GroupA.clearLayers();
}
function clearTypeB(){
    GroupB.clearLayers();
}
function clearTypeC(){
    GroupC.clearLayers();
}
function clearTypeD(){
    GroupD.clearLayers();
}
function clearTypeK(){
    GroupK.clearLayers();
}
function clearTypeL(){
    GroupL.clearLayers();
}

function clearTypeO(){
    GroupO.clearLayers();
}
function clearTypeP(){
    GroupP.clearLayers();
}
function clearTypeS(){
    GroupS.clearLayers();
}
function clearTypeT(){
    GroupT.clearLayers();
}
function clearTypeU(){
    GroupU.clearLayers();
}
function clearTypeV(){
    GroupV.clearLayers();
}
function clearTypeW(){
    GroupW.clearLayers();
}
function clearTypeX(){
    GroupX.clearLayers();
}
function clearTypeE() {
    GroupE.clearLayers();
}
function clearTypeF() {
    GroupF.clearLayers();
}
function clearTypeM() {
    GroupM.clearLayers();
}
function clearTypeN() {
    GroupN.clearLayers();
}
function clearTypeQ() {
    GroupQ.clearLayers();
}



function clearTypeOrg(){
    GroupOrg.clearLayers();
}
function clearTypeSrc(){
    GroupSrc.clearLayers();
}
function clearTypeDst(){
    GroupDst.clearLayers();
}


// drop ------------------

// cursor
function dropCursorPin(latitude, longitude, when){
	var marker = L.marker([latitude, longitude], { icon: iconCursor }).addTo(cursorGroup);
}

// kml
function dropKmlPin(latitude, longitude, when) {
    var marker = L.marker([latitude, longitude], { icon: iconKml }).addTo(kmlGroup);
}

// icon
    function dropTypeA(latitude, longitude, when, no, address) {
	var marker = L.marker([latitude, longitude], { icon: iconA }).addTo(GroupA);
        marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
        marker.options.singleClickTimeout = 250;
        marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
        marker.on('dblclick', function (e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
}
function dropTypeB(latitude, longitude, when, no, address){
	var marker = L.marker([latitude, longitude], { icon: iconB }).addTo(GroupB);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
    marker.options.singleClickTimeout = 250;
    marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
    marker.on('dblclick', function(e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
}
function dropTypeC(latitude, longitude, when, no, address){
	var marker = L.marker([latitude, longitude], { icon: iconC }).addTo(GroupC);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
    marker.options.singleClickTimeout = 250;
    marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
    marker.on('dblclick', function(e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
}
function dropTypeD(latitude, longitude, when, no, address){
	var marker = L.marker([latitude, longitude], { icon: iconD }).addTo(GroupD);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
    marker.options.singleClickTimeout = 250;
    marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
    marker.on('dblclick', function(e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
}
function dropTypeK(latitude, longitude, when, no, address){
	var marker = L.marker([latitude, longitude], { icon: iconK }).addTo(GroupK);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
    marker.options.singleClickTimeout = 250;
    marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
    marker.on('dblclick', function(e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
}
function dropTypeL(latitude, longitude, when, no, address){
	var marker = L.marker([latitude, longitude], { icon: iconL }).addTo(GroupL);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
    marker.options.singleClickTimeout = 250;
    marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
    marker.on('dblclick', function(e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
}
function dropTypeO(latitude, longitude, when, no, address){
	var marker = L.marker([latitude, longitude], { icon: iconO }).addTo(GroupO);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
    marker.options.singleClickTimeout = 250;
    marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
    marker.on('dblclick', function(e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
}
function dropTypeP(latitude, longitude, when, no, address){
	var marker = L.marker([latitude, longitude], { icon: iconP }).addTo(GroupP);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
    marker.options.singleClickTimeout = 250;
    marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
    marker.on('dblclick', function(e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
}
function dropTypeS(latitude, longitude, when, no, address){
	var marker = L.marker([latitude, longitude], { icon: iconS }).addTo(GroupS);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
    marker.options.singleClickTimeout = 250;
    marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
    marker.on('dblclick', function(e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
}
function dropTypeT(latitude, longitude, when, no, address){
	var marker = L.marker([latitude, longitude], { icon: iconT }).addTo(GroupT);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
    marker.options.singleClickTimeout = 250;
    marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
    marker.on('dblclick', function(e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
}
function dropTypeU(latitude, longitude, when, no, address){
	var marker = L.marker([latitude, longitude], { icon: iconU }).addTo(GroupU);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
    marker.options.singleClickTimeout = 250;
    marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
    marker.on('dblclick', function(e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
}
function dropTypeV(latitude, longitude, when, no, address){
	var marker = L.marker([latitude, longitude], { icon: iconV }).addTo(GroupV);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
    marker.options.singleClickTimeout = 250;
    marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
    marker.on('dblclick', function(e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
}
function dropTypeW(latitude, longitude, when, no, address){
	var marker = L.marker([latitude, longitude], { icon: iconW }).addTo(GroupW);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
    marker.options.singleClickTimeout = 250;
    marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
    marker.on('dblclick', function(e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
}
function dropTypeX(latitude, longitude, when, no, address){
	var marker = L.marker([latitude, longitude], { icon: iconX }).addTo(GroupX);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
    marker.options.singleClickTimeout = 250;
    marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
    marker.on('dblclick', function(e) {
        chrome.webview.hostObjects.class.IconDClick(no);
    });
    }
function dropTypeE(latitude, longitude, when, no, address) {
        var marker = L.marker([latitude, longitude], { icon: iconE }).addTo(GroupE);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
        marker.options.singleClickTimeout = 250;
        marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
        marker.on('dblclick', function (e) {
            chrome.webview.hostObjects.class.IconDClick(no);
        });
    }
function dropTypeF(latitude, longitude, when, no, address) {
        var marker = L.marker([latitude, longitude], { icon: iconF }).addTo(GroupF);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
        marker.options.singleClickTimeout = 250;
        marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
        marker.on('dblclick', function (e) {
            chrome.webview.hostObjects.class.IconDClick(no);
        });
    }
function dropTypeQ(latitude, longitude, when, no, address) {
        var marker = L.marker([latitude, longitude], { icon: iconQ }).addTo(GroupQ);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
        marker.options.singleClickTimeout = 250;
        marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
        marker.on('dblclick', function (e) {
            chrome.webview.hostObjects.class.IconDClick(no);
        });
    }
function dropTypeM(latitude, longitude, when, no, address) {
        var marker = L.marker([latitude, longitude], { icon: iconM }).addTo(GroupM);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
        marker.options.singleClickTimeout = 250;
        marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
        marker.on('dblclick', function (e) {
            chrome.webview.hostObjects.class.IconDClick(no);
        });
    }
function dropTypeN(latitude, longitude, when, no, address) {
        var marker = L.marker([latitude, longitude], { icon: iconN }).addTo(GroupN);
    marker.bindPopup(popupContent2(latitude, longitude, when, no, address));
        marker.options.singleClickTimeout = 250;
        marker.on('click', function (e) { chrome.webview.hostObjects.class.IconClick(no); });
        marker.on('dblclick', function (e) {
            chrome.webview.hostObjects.class.IconDClick(no);
        });
    }

function dropTypeOrg(latitude, longitude){
	var marker = L.marker([latitude, longitude], { icon: iconOrg }).addTo(GroupOrg);
}
function dropTypeSrc(latitude, longitude){
	var marker = L.marker([latitude, longitude], { icon: iconSrc }).addTo(GroupSrc);
}
function dropTypeDst(latitude, longitude){
	var marker = L.marker([latitude, longitude], { icon: iconDst }).addTo(GroupDst);
}


function popupContent(lat, lon, when, no) {
    var str = "<b>" + no + "</b>" +
        "<p>時刻: " + when + "<br>緯度: " + lat + "<br>経度: " + lon + "</p>" +
        "<a href=\"javascript:void(0)\" onClick=\"onExcelClick('"+no+"');\">Excel</a>";
    return str;
}

function popupContent2(lat, lon, when, no, address) {
    var str = "<b>" + no + "</b>" +
        "<p>時刻: " + when + "<br>緯度: " + lat + "<br>経度: " + lon + "<br>住所：" + address + "</p>" +
        "<a href=\"javascript:void(0)\" onClick=\"onExcelClick('" + no + "');\">Excel</a>";
    return str;
}

function onExcelClick(no) {
    chrome.webview.hostObjects.class.onExcelClick(no);
}

function setIconSize(e) {
  var i;
  var zoomLevel = mymap.getZoom();
    var x = zoomLevel * 16 / 18 * 2;
    var y = zoomLevel * 16 / 18 * 2;
  var iconElements = document.getElementsByClassName('icon');
  for (i = 0; i < iconElements.length; i++) {
     iconElements[i].style.width = Math.round(x) + 'px';
     iconElements[i].style.height = Math.round(y) + 'px';
  }
};