window.addEventListener('load', function () {

    myFetch('/api/v0.999/Admin', 'GET', true, data)
        .then(response => {
            if (!response.ok) {
                throw new Error(response.status)
            }
            return response
        })
        .then(data => {
            alert('yas');
            console.log(data)
        })
        .catch(error => {
            console.log(error);
            registerStatus.innerText = "Failed to gather users";
            document.location.href = "#";
        });
})



document.getElementById("registerButton").addEventListener("click", e => {
    var registerStatus = document.getElementById('registerStatus');
    registerStatus.innerText = "";
    var name = document.getElementById("Name").value.trim();
    var email = document.getElementById("email").value.trim();
    var username = document.getElementById("username").value.trim();
    var password = document.getElementById("password").value;
    var confirmPassword = document.getElementById("confirmPassword").value;
    if (password != confirmPassword) {
        registerStatus.innerText = "Password and Confirm Password Did Not Match";
        return;
    }

    data = { 'name': name, 'email': email, 'username': username, 'password': password };
    myFetch('/api/v0.999/Register', 'PUT', false, data)
        .then(response => {
            if (!response.ok) {
                throw new Error(response.status);
            }
        })
        .then(data => {
            alert('Registration was successful! Please log in.');
            document.location.href = "/";
        })
        .catch(error => {
            console.log(error);
            registerStatus.innerText = "Registration Failed";
            document.location.href = "#";
        });
});