$(document).ready(function () {
    'use strict';
    var paramRules = {
        txtPassword: {
            required: true,
            minlength: 5
        },
        txtConfirm: {
            required: true,
            equalTo: "#txtPassword"
        }
    };

    validForm(paramRules);

    $("body").on("click", "#btnSubmit", function () {
        if (checkValid() == true) {
            return recoveryPassword();
        }
        else {
            return false;
        }
        //return checkValid();
    });
});

//#region AutoComplete JS
function validForm(paramRules) {
    $('form').validate({
        rules: paramRules,
        highlight: function (element) {
            $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
        },
        success: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        }
    });
}
//#endregion AutoComplete JS

function checkValid() {
    if (!$("form").valid()) {
        return false;
    }
    else {
        return true;
    }
}

function recoveryPassword() {
    var jsonData = {
        email: $("#hdfRecoveryEmail").val(),
        recovery_code: $("#hdfRecoveryCode").val(),
        new_password: $("#txtPassword").val()
    };

    var url_action = 'member/ResetPassword';
    var account_type = $("#hdfTypeAccount").val();
    if (typeof account_type === 'undefined' || account_type == null || account_type == '')
        account_type = 'member';
    if (account_type != 'member')
        url_action = account_type + '/ResetPassword';
    
    $.ajax({
        type: "POST",
        //dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: url_action, // url of Api controller not mvc
        data: JSON.stringify(jsonData),
        success: function (data) {
            $("#wrap_error, #error_message").show();
            $("#error_message").html(data.message);
            if (data.errorCode == 0) {
                $("#error_message").css("color", "rgba(51, 122, 183, 0.77)");
                $("#btnSubmit, #txtPassword, #txtConfirm").attr('disabled', 'disabled');
            }
            else
            {
                $("#error_message").css("color", "#a94442");
            }
        },
        error: function (xhr, textStatus, err) {
            console.log("readyState: " + xhr.readyState);
            console.log("responseText: " + xhr.responseText);
            console.log("status: " + xhr.status);
            console.log("text status: " + textStatus);
            console.log("error: " + err);
        }
    });
    return false;
}
