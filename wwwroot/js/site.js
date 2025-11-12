// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function GetJSDistricts(eleid) {
    $.getJSON('/API/Masters/Districts', function (data) {
        debugger;
        let items = '<option value="">Select</option>';
        $.each(data, function (i, item) {
            items += '<option value="' + item.districtId_pk + '">' + item.districtName + '</option>';
        });
        $('#' + eleid).html(items);
    });
}
function GetJSBlocks(eleid, Paraid) {
    $.getJSON('/API/Masters/Blocks?DistrictId=' + Paraid, function (data) {
        debugger;
        let items = '<option value="">Select</option>';
        $.each(data, function (i, item) {
            items += '<option value="' + item.blockId_pk + '">' + item.blockName + '</option>';
        });
        $('#' + eleid).html(items);
    });
}
function GetJSPanchayats(eleid, Paraid1, Paraid2) {
    $.getJSON('/API/Masters/Panchayats?DistrictId=' + Paraid1 + "&&BlockId=" + Paraid2, function (data) {
        debugger;
        let items = '<option value="">Select</option>';
        $.each(data, function (i, item) {
            items += '<option value="' + item.panchayatId_pk + '">' + item.panchayatName + '</option>';
        });
        $('#' + eleid).html(items);
    });
}
function GetJSFederations(eleid, Paraid1, Paraid2) {
    $.getJSON('/API/Masters/Federations?DistrictId=' + Paraid1 + "&&BlockId=" + Paraid2, function (data) {
        debugger;
        let items = '<option value="">Select</option>';
        $.each(data, function (i, item) {
            items += '<option value="' + item.federationId_pk + '">' + item.federationName + '</option>';
        });
        $('#' + eleid).html(items);
    });
}

