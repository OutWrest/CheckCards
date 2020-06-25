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

    //addUserSubmit

    var submit = document.getElementById("addUserSubmit");

    submit.addEventListener("click", e => {
        var email = document.getElementById("addUserEmail").value;
        var username = document.getElementById("addUserUsername").value;
        var name = document.getElementById("addUserName").value;
        var password = document.getElementById("addUserPassword").value;

        data = { 'id': password, 'email': email, 'username': username, 'name': name };

        console.log(data);

        myFetch('/api/v0.999/Admin/CreateUser', 'PUT', true, data)
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.status);
                }
            })
            .then(data => {
                alert('User successfully created');
                location.reload();
            })
            .catch(error => {
                console.log(error);
                alert('Failed to create user');
            });

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

        myFetch('/api/v0.999/Admin/ResetPassword/' + _id, 'POST', true, data)
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.status);
                }
            })
            .then(data => {
                alert('Password reset was successful, email sent out to user.');
                location.reload();
            })
            .catch(error => {
                console.log(error);
                adminStatus.innerText = "Failed to reset the password";
                document.location.href = "#";
            });
    });

    userTemplate.children.deleteUserContainer.children.deleteUser.addEventListener("click", e => {
        var adminStatus = document.getElementById('adminStatus');
        adminStatus.innerText = "";

        var _id = userTemplate.children.Id.innerHTML;
        var _email = userTemplate.children.Email.innerHTML;
        var _username = userTemplate.children.UserName.innerHTML;
        var _name = userTemplate.children.Name.innerHTML;

        data = { 'id': _id, 'email': _email, 'username': _username, 'name': _name };

        console.log(data);

        myFetch('/api/v0.999/Admin/DeleteUser/' + _id, 'POST', true, data)
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.status);
                }
            })
            .then(data => {
                alert('User sucessfully deleted');
                location.reload();
            })
            .catch(error => {
                console.log(error);
                adminStatus.innerText = "Failed to delete user";
                document.location.href = "#";
            });
    });

    userTemplate.children.saveChangesContainer.children.saveChanges.addEventListener("click", e => {
        var adminStatus = document.getElementById('adminStatus');
        adminStatus.innerText = "";

        var _id = userTemplate.children.Id.innerHTML;
        var _email = userTemplate.children.Email.innerHTML;
        var _username = userTemplate.children.UserName.innerHTML;
        var _name = userTemplate.children.Name.innerHTML;

        data = { 'id': _id, 'email': _email, 'username': _username, 'name': _name };

        console.log(data);

        myFetch('/api/v0.999/Admin/SaveChanges/' + _id, 'POST', true, data)
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.status);
                }
            })
            .then(data => {
                alert('Changes successfully changed');
                location.reload();
            })
            .catch(error => {
                console.log(error);
                adminStatus.innerText = "Failed to change user attributes";
                document.location.href = "#";
            });
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
