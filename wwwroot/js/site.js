// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function GetJSDistricts(eleid) {
    $.getJSON('/API/Masters/Districts', function (data) {
        let items = '<option value="">Select</option>';
        $.each(data, function (i, item) {
            items += '<option value="' + item.districtId_pk + '">' + item.districtName + '</option>';
        });
        $('#' + eleid).html(items);
    });
}
function GetJSBlocks(eleid, Paraid) {
    $.getJSON('/API/Masters/Blocks?DistrictId=' + Paraid, function (data) {
        let items = '<option value="">Select</option>';
        $.each(data, function (i, item) {
            items += '<option value="' + item.blockId_pk + '">' + item.blockName + '</option>';
        });
        $('#' + eleid).html(items);
    });
}
function GetJSPanchayats(eleid, Paraid1, Paraid2) {
    $.getJSON('/API/Masters/Panchayats?DistrictId=' + Paraid1 + "&&BlockId=" + Paraid2, function (data) {
        let items = '<option value="">Select</option>';
        $.each(data, function (i, item) {
            items += '<option value="' + item.panchayatId_pk + '">' + item.panchayatName + '</option>';
        });
        $('#' + eleid).html(items);
    });
}
function GetJSFederations(eleid, Paraid1, Paraid2) {
    $.getJSON('/API/Masters/Federations?DistrictId=' + Paraid1 + "&&BlockId=" + Paraid2, function (data) {
        let items = '<option value="">Select</option>';
        $.each(data, function (i, item) {
            items += '<option value="' + item.federationId_pk + '">' + item.federationName + '</option>';
        });
        $('#' + eleid).html(items);
    });
}

function GetCampCode(eleid) {
    $.getJSON('/API/Masters/CampCode', function (data) {
        let items = '<option value="">Select</option>';
        $.each(data, function (i, item) {
            items += '<option value="' + item.campId_pk + '">' + item.campCode + '</option>';
        });
        $('#' + eleid).html(items);
    });
}

function GetOccupations(eleid) {
    $.getJSON('/API/Masters/Occupations', function (data) {
        let items = '<option value="">Select</option>';
        $.each(data, function (i, item) {
            items += '<option value="' + item.pk_OccupationId + '">' + item.occupatioName + '</option>';
        });
        $('#' + eleid).html(items);
    });
}

function GetPowerofGlasses(eleid) {
    $.getJSON('/API/Masters/PowerofGlasses', function (data) {
        let items = '<option value="">Select</option>';
        $.each(data, function (i, item) {
            items += '<option value="' + item.pk_PowerGlassId + '">' + item.powerofGlass + '</option>';
        });
        $('#' + eleid).html(items);
    });
}

function GetClfName(eleid) {
    $.getJSON('/API/Masters/ClfName', function (data) {
        let items = '<option value="">Select</option>';
        $.each(data, function (i, item) {
            items += '<option value="' + item.pk_CLFId + '">' + item.clfName + '</option>';
        });
        $('#' + eleid).html(items);
    });
}
