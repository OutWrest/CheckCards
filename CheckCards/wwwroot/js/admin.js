window.addEventListener('load', function () {
    var adminStatus = document.getElementById('adminStatus');

    window.template = document.getElementById('userTemplate').cloneNode(1);

    document.getElementById('usersTable').removeChild(document.getElementById('userTemplate'));

    myFetch('/api/v0.999/Admin/getUsers', 'GET', true, {})
        .then(response => {
            if (!response.ok) {
                throw new Error(response.status)
            }
            return response.json()
        })
        .then(jsonData => {
            createTable(jsonData)
            //console.log(data.json())
        })
        .catch(error => {
            console.log(error);
            adminStatus.innerText = "Failed to gather users";
            document.location.href = "#";
        });
})

function createRow(number, id, username, name, email) {
    var table = document.getElementById('usersTable');
    var userTemplate = window.template.cloneNode(1);

    userTemplate.children.Number.innerHTML = number.toString();
    userTemplate.children.Id.innerHTML = id;
    userTemplate.children.UserName.innerHTML = username;
    userTemplate.children.Name.innerHTML = name;
    userTemplate.children.Email.innerHTML = email;

    userTemplate.children.resetPasswordContainer.children.resetPassword.addEventListener("click", e => {
        var adminStatus = document.getElementById('adminStatus');
        adminStatus.innerText = "";

        var _id = userTemplate.children.Id.innerHTML;
        var _email = userTemplate.children.Email.innerHTML;
        var _username = userTemplate.children.UserName.innerHTML;
        var _name = userTemplate.children.Name.innerHTML;

        data = { 'id': _id, 'email': _email, 'username': _username, 'name': _name };

        console.log(data);
    });

    table.appendChild(userTemplate);
}

function createTable(jsonData) {
    var i;
    var user;
    for (i = 0; i < jsonData.length; i++) {
        user = jsonData[i];
        console.log(user);
        createRow(i, user.id, user.userName, user.name, user.email);
    }
}


// Button actions
/*
document.getElementById("resetPassword").addEventListener("click", e => {
    var adminStatus = document.getElementById('adminStatus');
    adminStatus.innerText = "";

    
    var confirmPassword = document.getElementById("confirmPassword").value;
    if (password != confirmPassword) {
        registerStatus.innerText = "Password and Confirm Password Did Not Match";
        return;
    }

    data = { 'name': name, 'email': email, 'username': username, 'password': password };
    myFetch('/api/v0.999/Admin/resetPassword/' + id, 'PUT', false, data)
        .then(response => {
            if (!response.ok) {
                throw new Error(response.status);
            }
        })
        .then(data => {
            alert('Password reset was successful, email sent out to user.');
        })
        .catch(error => {
            console.log(error);
            adminStatus.innerText = "Failed to reset the password";
            document.location.href = "#";
        });
});
*/