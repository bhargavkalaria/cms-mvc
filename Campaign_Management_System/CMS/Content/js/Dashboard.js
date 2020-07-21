$(document).ready(function () {
     toastr.options = {
        "closeButton": false,
        "positionClass": "toast-bottom-right",
        "showDuration": "3000",
        "hideDuration": "1000",
        "timeOut": "3000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    $('#reset').click(function () {
        if (!$('#cur_pwd').val()) {
            toastr.error('Please Enter Password');
        } else if (!$('#new_pwd').val()) {
            toastr.error('Please Enter New Password');
        } 
        else {
            $.ajax({
                url: '/Login/ResetPassword',
                type: 'post',
                data: {
                    cur_pwd: $('#cur_pwd').val(),
                    new_pwd: $('#new_pwd').val(),
                },
                success: function (result) {
                    $("#exampleModal").modal("hide");
                    toastr.success('Your password is change successfully');
                }, error: function (error) {
                    toastr.error('Error in updating password');
                }
            });
        }
    });

});