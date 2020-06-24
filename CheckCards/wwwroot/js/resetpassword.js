document.getElementById("passwordButton").addEventListener("click", e => {
    var passwordStatus = document.getElementById('passwordStatus');
    passwordStatus.innerText = "";
    var password = document.getElementById("password").value;
    var confirmPassword = document.getElementById("confirmPassword").value;
    var pathname = window.location.pathname.split('/');
    var id = pathname[pathname.length-1]

    if (password != confirmPassword) {
        passwordStatus.innerText = "Password and Confirm Password Did Not Match";
        return;
    }

    data = { 'newpassword': password };
    myFetch('/api/v0.999/ResetPassword/' + id, 'POST', false, data)
        .then(response => {
            if (!response.ok) {
                throw new Error(response.status);
            }
        })
        .then(data => {
            alert('Password Reset was successful! Please log in.');
            console.log(data)
            //document.location.href = "/";
        })
        .catch(error => {
            console.log(error);
            passwordStatus.innerText = "Password Reset Failed";
            //document.location.href = "#";
        });
});