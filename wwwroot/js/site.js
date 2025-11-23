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
    $.getJSON('/API/Masters/Blocks?DistrictId=' + Paraid + '&ModuleId='+ModuleId, function (response) {
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

function GetCampCode(eleid) {
    $('#' + eleid).empty();
    $.getJSON('/API/Masters/CampCode', function (response) {
        console.log('CampCode API Response:', response);
        
        if (response.status && response.data) {
            console.log('CampCode Data:', response.data);
            console.log('First Item:', response.data[0]);
            console.log('First Item Keys:', Object.keys(response.data[0] || {}));
            
            let items = '<option value="">Select</option>';
            $.each(response.data, function (i, item) {
                // Log the first item to see actual structure
                if (i === 0) {
                    console.log('Processing first item:', item);
                    console.log('campId_pk:', item.campId_pk);
                    console.log('CampId_pk:', item.CampId_pk);
                    console.log('campCode:', item.campCode);
                    console.log('CampCode:', item.CampCode);
                }
                
                // Handle both camelCase and PascalCase (check all possible property name variations)
                var campId = item.campId_pk || item.CampId_pk || item.campIdPk || item.CampIdPk;
                var campCode = item.campCode || item.CampCode;
                
                if (!campId || !campCode) {
                    console.error('Missing property in item:', item);
                    return; // Skip this item if properties are missing
                }
                
                items += '<option value="' + campId + '">' + campCode + '</option>';
            });
            
            console.log('Generated options HTML length:', items.length);
            
            // Temporarily enable dropdown to set HTML, then restore disabled state if needed
            var wasDisabled = $('#' + eleid).prop('disabled');
            $('#' + eleid).prop('disabled', false);
            $('#' + eleid).html(items);
            if (wasDisabled) {
                $('#' + eleid).prop('disabled', true);
            }
            
            console.log('CampCode dropdown populated with', $('#' + eleid + ' option').length, 'options');
            
            // Trigger custom event after dropdown is populated
            $('#' + eleid).trigger('campcode:loaded');
        } else {
            console.error('CampCode API response error:', response);
        }
    }).fail(function(jqXHR, textStatus, errorThrown) {
        console.error('CampCode API request failed:', textStatus, errorThrown);
        console.error('Response:', jqXHR.responseText);
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
            // Trigger custom event after dropdown is populated
            $('#' + eleid).trigger('occupations:loaded');
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
            // Trigger custom event after dropdown is populated
            $('#' + eleid).trigger('powerofglasses:loaded');
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
                items += '<option value="' + item.pk_VendorsId + '">' + item.VEName + '</option>';
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

