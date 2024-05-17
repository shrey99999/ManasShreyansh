$(document).ready(function () {
    $('#btnDelete').click(function () {
        var chkSelection = 0;

        $('input[id^="chk_"]').each(function () {
            if ($(this).is(':checked')) {
                chkSelection = 1
            }
        });
        
        if (chkSelection == 0) {
            swal({

                text: 'Please Select at Least one Record to Delete.',
                type: 'error',
            });
            return; // Prevent further execution if no checkbox is selected
        }


        swal({
            type:'warning',
            text: 'Are you sure you want to delete this item?',
            showDenyButton: true,
            showCancelButton: true,
            confirmButtonText: 'Yes',
            denyButtonText: 'No'
        }).then((result) => {
            if (result.value) {

                var ids = '';
                var count = 0;
                $('input[id^="chk_"]').each(function () {
                    if ($(this).is(":checked")) {
                        var id = $(this).attr('id').split('_')[1]
                        if (count == 0) {
                            ids = id;
                            count = count + 1;
                        }
                        else {
                            ids = ids + "," + id;
                        }
                    }
                })

                $.ajax({
                    type: 'POST',
                    url: '/Sales/DeleteData',
                    //url: '/RDSO/SaveUserMaster',
                    /*contentType: 'application/json',*/
                    /*dataType: 'json',*/
                    data: { id: ids, table_name: $('#table_name').val() },
                    success: function (result) {
                        if (result.status == an.type.error) {
                            toastr.error(result.msg, 'Sorry')
                        }
                        if (result.status > 0) {
                            swal({
                                text: result.msg,
                                type: 'success',
                            }).then(function () {
                                reload();
                            });
                        }
                    },
                    statusCode: {
                        500: function () {
                            toastr.error('Server error', 'Sorry')
                        },
                        0: function () {
                            toastr.error('Internet Connection was broken', 'Sorry')
                        }
                    },
                    error: function (xhr, result) {
                        toastr.error(result, 'Sorry')
                        if (result === 'parsererror') {
                            /*reload();*/
                        }
                    },
                    complete: function () {
                        loader.remove();
                    }
                });
            }
        });
    });
});

















    //$('#btnDelete').click(function () {

    //    var chkSelection = 0;

    //    $('input[id^="chk_"]').each(function () {
    //        if ($(this).is(':checked')) {
    //            chkSelection = 1
    //        }
    //    });

    //    if (chkSelection == 0) {
    //        swal({
    //            text: 'Please select record.',
    //            type: 'error',
    //        });
    //        return;
    //    };

    //    var ids = '';
    //    var count = 0;
    //    $('input[id^="chk_"]').each(function () {
    //        if ($(this).is(":checked")) {
    //            var id = $(this).attr('id').split('_')[1]
    //            if (count == 0) {
    //                ids = id;
    //                count = count + 1;
    //            }
    //            else {
    //                ids = ids + "," + id;
    //            }
    //        }
    //    });


    //    $.ajax({
    //        type: 'POST',
    //        url: '/Sales/DeleteData',
    //        //url: '/RDSO/SaveUserMaster',
    //        /*contentType: 'application/json',*/
    //        /*dataType: 'json',*/
    //        data: { id: ids, table_name: $('#table_name').val() },
    //        success: function (result) {
    //            if (result.status == an.type.error) {
    //                toastr.error(result.msg, 'Sorry')
    //            }
    //            if (result.status > 0) {
    //                mdlA.dispose();
    //                swal({
    //                    text: result.msg,
    //                    type: 'success',
    //                }).then(function () {
    //                    reload();
    //                });
    //            }
    //        },
    //        statusCode: {
    //            500: function () {
    //                toastr.error('Server error', 'Sorry')
    //            },
    //            0: function () {
    //                toastr.error('Internet Connection was broken', 'Sorry')
    //            }
    //        },
    //        error: function (xhr, result) {
    //            toastr.error(result, 'Sorry')
    //            if (result === 'parsererror') {
    //                /*reload();*/
    //            }
    //        },
    //        complete: function () {
    //            loader.remove();
    //        }
    //    });
    //});
