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

    $('#login').click(function () {
        if (!$('#Email').val()) {
            toastr.error('Please enter email')
        } else if (!$('#Password').val()) {
            toastr.error('Please enter password')
        } else {
            $.ajax({
                url: 'Login',
                method: 'post',
                data: {
                    Password: $('#Password').val(),
                    Email: $('#Email').val(),
                },
                success: function (result) {
                    console.log(result)
                }, error: function (error) {
                    console.log(error)
                }
            });
        }
    });

   
    $('#add_user').click(function () {
        if (!$('#Email').val()) {
            toastr.error('Please enter email')
        } else if (!$('#Password').val()) {
            toastr.error('Please enter password')
        } else {
            $.ajax({
                url: 'Login/AddUser',
                method: 'post',
                data: {
                    Password: $('#Password').val(),
                    Email: $('#Email').val(),
                    Role: $('#Role').val(),
                    FName: $('#FName').val(),
                    LName:$('#LName').val(),
                },
                success: function (result) {
                    console.log(result)
                }, error: function (error) {
                    console.log(error)
                }
            });
        }
    });

    $('#forgotpassword').click(function () {
        if (!$('#Email').val()) {
            toastr.error('Please enter email')
        }else {
            $.ajax({
                url: 'Login/ForgotPassword',
                method: 'post',
                data: {
                    Email: $('#Email').val(),
                },
                success: function (result) {
                    console.log(result)
                }, error: function (error) {
                    console.log(error)
                }
            });
        }
    })
});  