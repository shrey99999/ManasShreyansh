var date = new Date();
var day = date.getDate();
var month = date.getMonth() + 1;
var year = date.getFullYear();
if (month < 10) month = "0" + month;
if (day < 10) day = "0" + day;
var today = day + "/" + month + "/" + year;

var modalAlert = {
    title: '',
    content: '',
    confirmContent: '<h5>Are you sure?</h5>',
    parent: $('body'),
    id: 'mymodal',
    size: { small: 'modal-sm', large: 'modal-lg', xlarge: 'modal-xl', xxlarge: 'modal-xxl', default: '' },
    div: '<div class="modal fade" id={id} tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">'
        + '<div class= "modal-dialog modal-dialog-centered" role="document"><div class="modal-content"><div class="modal-header">'
        + '<h5 class="modal-title"></h5><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button></div><div class="modal-body"></div><div class="modal-footer">'
        + '<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>'
        + '</div></div></div></div >',
    divAlert: '<div class="modal fade" id={id} tabindex="-1" role="dialog" aria-hidden="true">'
        + '<div class= "modal-dialog modal-dialog-centered" role="document">'
        + '<div class="modal-content"><div class="modal-body"></div></div></div></div >',
    show: function (size) {
        var mdlId = this.id;
        this.parent.append(this.div.replace('{id}', mdlId));
        $('#' + mdlId + ' .modal-title').html(this.title);
        $('#' + mdlId + ' .modal-body').html(this.content);
        $('#' + mdlId + ' .modal-dialog').addClass(size);
        $('#' + mdlId).modal(this.options);
    },
    alert: function (size) {
        var mdlId = this.id;
        this.parent.append(this.divAlert.replace('{id}', mdlId));
        $('#' + mdlId + ' .modal-body').html(this.content);
        $('#' + mdlId + ' .modal-dialog').addClass(size);
        $('#' + mdlId).modal(this.options);
    },
    options: { backdrop: true, keyboard: true, focus: true, show: true },
    dispose: function (f) {
        var mdlId = this.id;
        $('#' + mdlId + ' .modal-content').animate({ opacity: 0 }, 500, function () {
            $('#' + mdlId + ',.modal-backdrop').remove();
            $('body').removeClass('modal-open').removeAttr('style');
            if (f !== undefined)
                f();
        });
    },
    anim: function (ms) {
        $('#' + this.id + ' .modal-content').animate({ opacity: 0 }, ms);
        $('#' + this.id + ' .modal-content').animate({ opacity: 1 }, ms);
    },
    confirm: function () {
        return '<div class="col-md-12" id="dvpopup">'
            + '<button type = "button" class="close" aria-label="Close">'
            + '<span aria-hidden="true">&times;</span></button>'
            + this.confirmContent
            + '<div class="form-group"></div> <button class="btn btn-outline-success mr-2" id="btnOK">Yes</button>'
            + '<button class="btn btn-outline-danger" id="mdlCancel">No</button></div>'
    }
};
var alertNormal = {
    title: '',
    content: '',
    color: { green: 'alert-success', red: 'alert-danger', blue: 'alert-info', warning: 'alert-warning' },
    linkClass: 'alert-link',
    iclass: { failed: 'fas fa-times-circle', warning: 'fas fa-exclamation-triangle', success: 'fas fa-check-circle', info: 'fas fa-info-circle' },
    type: { failed: -1, warning: 0, success: 1, info: 2 },
    parent: $('#alertmsg'),
    id: 'alert',
    div: '<div id={id} class="alert {color} alert-dismissible fade" role="alert">'
        + '<strong > <i class="{iclass}"></i> {title}!</strong> {content}'
        + '<button type="button" class= "close pr-2" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button ></div>',
    alert: function (type) {
        var cls = this.color.blue;
        if (type === this.type.success) cls = this.color.green;
        else if (type === this.type.failed) cls = this.color.red;
        else if (type === this.type.warning) cls = this.color.warning;
        var icls = this.iclass.info;
        if (type === this.type.success) icls = this.iclass.success;
        else if (type === this.type.failed) icls = this.iclass.failed;
        else if (type === this.type.warning) icls = this.iclass.warning;
        this.parent.html(this.div.replace('{id}', this.id).replace('{title}', this.title).replace('{content}', this.content).replace('{color}', cls).replace('{iclass}', icls));
        this.show();
        if (this.autoClose > 0) {
            setTimeout(function () {
                alertNormal.close();
            }, this.autoClose * 1000);
        }
    },
    close: function () {
        $('#' + this.id).removeClass('show');
    },
    show: function () {
        $('#' + this.id).addClass('show');
    },
    autoClose: 0,
    remove: function () {
        $('#' + this.id).remove();
    }
}
var mdlA = modalAlert;

var OpenDefaultModal = function (result) {
    mdlA.content = result;
    mdlA.id = 'mymodal';
    mdlA.options.backdrop = 'static';
    mdlA.alert(mdlA.size.default);
    $('button.close span,#mdlCancel').unbind().click(function () {
        mdlA.dispose();
    });
}

var OpenLargeModal = function (result) {
    mdlA.content = result;
    mdlA.id = 'mymodal';
    mdlA.options.backdrop = 'static';
    mdlA.alert(mdlA.size.large);
    $('button.close span,#mdlCancel').unbind().click(function () {
        mdlA.dispose();
    });
}

var an = alertNormal;


var preloader = {
    load: function () {
        $('body').append('<div class="spinner-border text-primary" role="status"><span class="sr-only">Loading...</span></div>');
    },
    remove: function () {
        $('.spinner-border').remove();
    }
};

var loader = {
    load: function () {
        $("#loader").css("display", "block");
    },
    remove: function () {
        $("#loader").css("display", "none");
    }
};

var btnloader = {
    load: function () {
        $('#btnSave').append('<span id="loadSpan" class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');
        $('#btnSave').attr("disabled", "disabled")
    },
    remove: function () {
        $('#loadSpan').remove();
        $('#btnSave').removeAttr('disabled');
    }
};

var loc = location.href.split('#')[0];
var reload = function () {
    location.href = loc;
};

//const Toast = Swal.mixin({
//    toast: true,
//    position: 'top-end',
//    showConfirmButton: false,
//    timer: 3000
//});

var $v = $validator;

var ViewProjectDetails = function (url, p_id, ma_user_id) {
    loader.load();
    $.post(url, { p_id: p_id, lid: ma_user_id }).done(function (result) {
        //resultReload(result);
        mdlA.content = result;
        mdlA.id = 'mymodal';
        mdlA.options.backdrop = 'static';
        mdlA.alert(mdlA.size.xlarge);
        $('button.close span,#mdlCancel').unbind().click(function () {
            mdlA.dispose();
        });
    }).catch(function (xhr, e, msg) {

    }).fail(function (xhr) {
        if (xhr.status == 500) {
            Toast.fire({
                icon: 'error',
                title: 'Server error'
            });
        }
        if (xhr.status == 0) {
            Toast.fire({
                icon: 'error',
                title: 'Internet Connection was broken'
            });
        }
    }).always(function () {
        loader.remove();
    });
};

var CheckAll = function () {
    $('#chkall').change(function () {
        if ($('#chkall').is(":checked")) {
            $('input[id^="chk_"]').each(function () {
                $(this).prop("checked", true);
            });
           /* $('#' + btn).css("display", "inline-block");*/
        }
        else {
            $('input[id^="chk_"]').each(function () {
                $(this).prop("checked", false);
            });
           /* $('#' + btn).css("display", "none");*/
        }
    });
}

var CheckOne = function () {
    $('input[id^="chk_"]').click(function () {
        var chkCount = $('input[id^="chk_"]').length;
        var chkStatus = 0;
        /*$('#' + btn).css("display", "none");*/
        $('input[id^="chk_"]').each(function () {
            if ($(this).is(':checked')) {
                chkStatus = chkStatus + 1;
                /*$('#' + btn).css("display", "inline-block");*/
            }
        });
        if (chkCount == chkStatus) {
            $('#chkall').prop("checked", true);
           /* $('#' + btn).css("display", "inline-block");*/
        }
        else {
            $('#chkall').prop("checked", false);
            /*$('#' + btn).css("display", "none");*/
        }
    });
}

var Revert = function (popupUrl, url, control) {
    var stage_id = $('#' + control).attr('data-stage-id');
    var userdocs_id = $('#' + control).attr('data-userdocs-id');
    var old_stage_id = $('#' + control).attr('data-old-stage-id');
    $.post(popupUrl, { stage_id: stage_id, userdocs_id: userdocs_id, old_stage_id: old_stage_id }).done(function (result) {
        //resultReload(result);
        mdlA.content = result;
        mdlA.id = 'mymodal';
        mdlA.options.backdrop = 'static';
        mdlA.alert(mdlA.size.default);

        $('button.close span,#mdlCancel').unbind().click(function () {
            mdlA.dispose();
        });

        $('.select2bs4').select2({
            theme: 'bootstrap4'
        })

        $("#txtdateofforwarding").attr("value", today);

        $("#btnSave").click(function () {
            var userdocs_id = $("#userdocs_id").val();
            var user_id = $("#user_id").val();
            var remark_received = $('#txtRemark').val();
            stage_id = $('#stage_id').val();
            old_stage_id = $('#old_stage_id').val();
            btnloader.load();
            $.ajax({
                type: "Post",
                contentType: "Application/Json",
                url: url,
                data: JSON.stringify({ userdocs_id: userdocs_id, remark_received: remark_received, user_id: user_id, stage_id: stage_id, old_stage_id: old_stage_id }),
                dataType: "json",
                success: function (result) {
                    Toast.fire({
                        icon: result.status == an.type.success ? 'success' : 'error',
                        title: result.msg
                    });
                    if (result.status == an.type.success) {
                        mdlA.dispose(function () {
                            reload();
                        });
                    }
                },
                complete: function () {
                    btnloader.remove();
                }
            });
        })

    }).catch(function (xhr, e, msg) {

    }).fail(function (xhr) {
        if (xhr.status == 500) {
            Toast.fire({
                icon: 'error',
                title: 'Server error'
            });
        }
        if (xhr.status == 0) {
            Toast.fire({
                icon: 'error',
                title: 'Internet Connection was broken'
            });
        }
    }).always(function () {
        loader.remove();
    });
}