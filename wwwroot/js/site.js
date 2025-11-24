// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function GetJSDistricts(eleid, ModuleId = 0) {
    $('#' + eleid).empty();
    $.getJSON('/API/Masters/Districts?ModuleId=' + ModuleId, function (response) {
        if (response.status && response.data) {
            let items = '<option value="">Select</option>';
            $.each(response.data, function (i, item) {
                items += '<option value="' + item.districtId_pk + '">' + item.districtName + '</option>';
            });
            $('#' + eleid).html(items);
        }
    });
}
function GetJSBlocks(eleid, Paraid, ModuleId = 0) {
    $('#' + eleid).empty();
    $.getJSON('/API/Masters/Blocks?DistrictId=' + Paraid + '&ModuleId=' + ModuleId, function (response) {
        if (response.status && response.data) {
            let items = '<option value="">Select</option>';
            $.each(response.data, function (i, item) {
                items += '<option value="' + item.blockId_pk + '">' + item.blockName + '</option>';
            });
            $('#' + eleid).html(items);
        }
    });
}
function GetJSPanchayats(eleid, Paraid1, Paraid2) {
    $('#' + eleid).empty();
    $.getJSON('/API/Masters/Panchayats?DistrictId=' + Paraid1 + "&&BlockId=" + Paraid2, function (response) {
        if (response.status && response.data) {
            let items = '<option value="">Select</option>';
            $.each(response.data, function (i, item) {
                items += '<option value="' + item.panchayatId_pk + '">' + item.panchayatName + '</option>';
            });
            $('#' + eleid).html(items);
        }
    });
}
function GetJSFederations(eleid, Paraid1, Paraid2) {
    $('#' + eleid).empty();
    $.getJSON('/API/Masters/Federations?DistrictId=' + Paraid1 + "&&BlockId=" + Paraid2, function (response) {
        if (response.status && response.data) {
            let items = '<option value="">Select</option>';
            $.each(response.data, function (i, item) {
                items += '<option value="' + item.federationId_pk + '">' + item.federationName + '</option>';
            });
            $('#' + eleid).html(items);
        }
    });
}

function GetCampCode(eleid, code, TypeMId,isselect) {
    //debugger;
    $('#' + eleid).empty(); let items;
    $.getJSON('/API/Masters/CampCode?code=' + code + '&&TypeMId=' + TypeMId, function (response) {
        if (response.status && response.data) {
            if (code == "" || code == "0" || code == undefined) {
                items = '<option value="">Select</option>';
            }
            //items = '<option value="">Select</option>';
            $.each(response.data, function (i, item) {
                items += '<option value="' + item.campId_pk + '">' + item.campCode + '</option>';
            });
            // Temporarily enable dropdown to set HTML, then restore disabled state if needed
            var wasDisabled = $('#' + eleid).prop('disabled');
            $('#' + eleid).prop('disabled', false);
            $('#' + eleid).html(items);
            if (wasDisabled) {
                $('#' + eleid).prop('disabled', true);
            }
        }
    });
}

function GetOccupations(eleid) {
    $('#' + eleid).empty();
    $.getJSON('/API/Masters/Occupations', function (response) {
        if (response.status && response.data) {
            let items = '<option value="">Select</option>';
            $.each(response.data, function (i, item) {
                items += '<option value="' + item.pk_OccupationId + '">' + item.occupatioName + '</option>';
            });
            $('#' + eleid).html(items);
        }
    });
}

function GetPowerofGlasses(eleid) {
    $('#' + eleid).empty();
    $.getJSON('/API/Masters/PowerofGlasses', function (response) {
        if (response.status && response.data) {
            let items = '<option value="">Select</option>';
            $.each(response.data, function (i, item) {
                items += '<option value="' + item.pk_PowerGlassId + '">' + item.powerofGlass + '</option>';
            });
            $('#' + eleid).html(items);
        }
    });
}

function GetClfName(eleid) {
    $('#' + eleid).empty();
    $.getJSON('/API/Masters/ClfName', function (response) {
        if (response.status && response.data) {
            let items = '<option value="">Select</option>';
            $.each(response.data, function (i, item) {
                items += '<option value="' + item.pk_CLFId + '">' + item.clfName + '</option>';
            });
            $('#' + eleid).html(items);
        }
    });
}
function GetJSVE(eleid, isSelect) {
    debugger;
    $('#' + eleid).empty();
    $.getJSON('/API/Masters/VEData?isSelect=' + isSelect, function (response) {
        if (response.status && response.data) {
            let items = '<option value="">Select</option>';
            $.each(response.data, function (i, item) {
                items += '<option value="' + item.pk_VendorsId + '">' + item.veName + '</option>';
            });
            $('#' + eleid).html(items);
        }
    });
}
function GetJSTypeofModuleData(eleid, isSelect) {
    $('#' + eleid).empty();
    $.getJSON('/API/Masters/DataTypeOfModule?isSelect=' + isSelect, function (response) {
        if (response.status && response.data) {
            let items = '<option value="">Select</option>';
            $.each(response.data, function (i, item) {
                items += '<option value="' + item.Value + '">' + item.Text + '</option>';
            });
            $('#' + eleid).html(items);
        }
    });
}

$nav = $('.vertical-sidebar');
$header = $('.app-content');
$toggle_nav_top = $('.header-toggle');
$toggle_nav_top.click(function () {
    $this = $(this);
    $nav = $('.vertical-sidebar');
    $nav.toggleClass('open');
    $header.toggleClass('open');
});

