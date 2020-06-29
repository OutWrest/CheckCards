window.addEventListener('load', function () {
    var adminStatus = document.getElementById('adminStatus');

    window.template = document.getElementById('userTemplate').cloneNode(1);

    document.getElementById('usersTable').removeChild(document.getElementById('userTemplate'));


    $('#modalLoadingScreen').modal('show');
    document.getElementById('modalDocumentContainer').classList.remove('modal-dialog'); // fixes dumb bootstrap error
    document.getElementById('modalDocumentContainer').classList.add('loadContainer');

    $('#modalLoadingScreen').on('shown.bs.modal', function (e) {
        myFetch('/api/v0.999/Admin/getUsers', 'GET', true, {})
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.status)
                }
                return response.json()
            })
            .then(jsonData => {
                $('#modalLoadingScreen').modal('hide');
                createTable(jsonData)
                //console.log(data.json())
            })
            .catch(error => {
                $('#modalLoadingScreen').modal('hide');
                console.log(error);
                adminStatus.innerText = "Failed to gather users";
                document.location.href = "#";
            });
        $('#modalLoadingScreen').off('shown.bs.modal');
    })

    var submit = document.getElementById("addUserSubmit");

    submit.addEventListener("click", e => {
        var email = document.getElementById("addUserEmail").value;
        var username = document.getElementById("addUserUsername").value;
        var name = document.getElementById("addUserName").value;
        var password = document.getElementById("addUserPassword").value;

        data = { 'id': password, 'email': email, 'username': username, 'name': name };

        console.log(data);

        $('#modalLoadingScreen').modal('show');
        myFetch('/api/v0.999/Admin/CreateUser', 'PUT', true, data)
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.status);
                }
            })
            .then(data => {
                $('#modalLoadingScreen').modal('hide');
                alert('User successfully created');
                location.reload();
            })
            .catch(error => {
                $('#modalLoadingScreen').modal('hide');
                console.log(error);
                alert('Failed to create user');
            });

    });
})

function createRow(number, id, username, name, email, roles) {
    var table = document.getElementById('usersTable');
    var userTemplate = window.template.cloneNode(1);

    userTemplate.children.Number.innerHTML = number.toString();
    userTemplate.children.Id.innerHTML = id;
    userTemplate.children.UserName.innerHTML = username;
    userTemplate.children.Name.innerHTML = name;
    userTemplate.children.Email.innerHTML = email;

    if (roles.includes("User")) {
        userTemplate.children.rolesContainer.children.roles.children.User.checked = true;
    }

    if (roles.includes("Administrator")) {
        userTemplate.children.rolesContainer.children.roles.children.Admin.checked = true;
    }
    

    userTemplate.children.resetPasswordContainer.children.resetPassword.addEventListener("click", e => {
        var adminStatus = document.getElementById('adminStatus');
        adminStatus.innerText = "";

        var _id = userTemplate.children.Id.innerHTML;
        var _email = userTemplate.children.Email.innerHTML;
        var _username = userTemplate.children.UserName.innerHTML;
        var _name = userTemplate.children.Name.innerHTML;

        data = { 'id': _id, 'email': _email, 'username': _username, 'name': _name };

        console.log(data);

        $('#modalLoadingScreen').modal('show');
        myFetch('/api/v0.999/Admin/ResetPassword/' + _id, 'POST', true, data)
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.status);
                }
            })
            .then(data => {
                $('#modalLoadingScreen').modal('hide');
                alert('Password reset was successful, email sent out to user.');
                location.reload();
            })
            .catch(error => {
                $('#modalLoadingScreen').modal('hide');
                console.log(error);
                adminStatus.innerText = "Failed to reset the password";
                document.location.href = "#";
            });
    });

    userTemplate.children.resetQuestionsContainer.children.resetQuestions.addEventListener("click", e => {
        var adminStatus = document.getElementById('adminStatus');
        adminStatus.innerText = "";

        var _id = userTemplate.children.Id.innerHTML;
        var _email = userTemplate.children.Email.innerHTML;
        var _username = userTemplate.children.UserName.innerHTML;
        var _name = userTemplate.children.Name.innerHTML;

        data = { 'id': _id, 'email': _email, 'username': _username, 'name': _name };

        console.log(data);

        $('#modalLoadingScreen').modal('show');
        myFetch('/api/v0.999/Admin/ResetQuestions/' + _id, 'POST', true, data)
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.status);
                }
            })
            .then(data => {
                $('#modalLoadingScreen').modal('hide');
                alert('Security questions were successfully reset');
                location.reload();
            })
            .catch(error => {
                $('#modalLoadingScreen').modal('hide');
                console.log(error);
                adminStatus.innerText = "Failed to reset the security questions";
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

        $('#modalLoadingScreen').modal('show');
        myFetch('/api/v0.999/Admin/DeleteUser/' + _id, 'POST', true, data)
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.status);
                }
            })
            .then(data => {
                $('#modalLoadingScreen').modal('hide');
                alert('User sucessfully deleted');
                location.reload();
            })
            .catch(error => {
                $('#modalLoadingScreen').modal('hide');
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
        var roles = []

        if (userTemplate.children.rolesContainer.children.roles.children.User.checked) {
            roles.push("User");
        }

        if (userTemplate.children.rolesContainer.children.roles.children.Admin.checked) {
            roles.push("Administrator");
        }


        data = { 'id': _id, 'email': _email, 'username': _username, 'name': _name, 'roles': roles };

        console.log(data);

        $('#modalLoadingScreen').modal('show');
        myFetch('/api/v0.999/Admin/SaveChanges/' + _id, 'POST', true, data)
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.status);
                }
            })
            .then(data => {
                $('#modalLoadingScreen').modal('hide');
                alert('Changes successfully changed');
                location.reload();
            })
            .catch(error => {
                $('#modalLoadingScreen').modal('hide');
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
        createRow(i, user.id, user.userName, user.name, user.email, user.roles);
    }
}
