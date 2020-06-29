// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function myFetch(url, method, authenticated, data = {}) {
    var requestInfo = {
        method: method,
        headers: { 'Content-Type': 'application/json' },
        cache: 'no-cache',
        credentials: 'same-origin',
        redirect: 'manual'
    };
    if (authenticated) {
        var token = sessionStorage.getItem('jwt');
        requestInfo.headers['Authorization'] = 'bearer ' + token;
    }
    if (method == 'POST' || method == 'PUT') {
        requestInfo.body = JSON.stringify(data);
    }

    return fetch(url, requestInfo);
}

function enableLoginSpinner(on) {
    var loginSpinner = document.getElementById('loginSpinner');
    if (on)
        loginSpinner.classList.remove('d-none');
    else
        loginSpinner.classList.add('d-none');
}

async function isLoggedIn() {
    var jwt = sessionStorage.getItem('jwt');
    if (jwt != null && jwt !== '') {
        myFetch('/api/v0.999/Internal', 'GET', true)
            .then(response => { setLoggedIn(response.ok); showHideBasedOnLogin() })
            .catch(exception => { setLoggedIn(false); showHideBasedOnLogin() });
    }
    else {
        setLoggedIn(false);
        showHideBasedOnLogin();
    }

}

async function isAdmin() {
    var jwt = sessionStorage.getItem('jwt');
    if (jwt != null && jwt !== '') {
        myFetch('/api/v0.999/Admin', 'GET', true)
            .then(response => { setAdmin(response.ok); showHideBasedOnLogin() })
            .catch(exception => { setAdmin(false); showHideBasedOnLogin() });
    }
    else {
        setAdmin(false);
        showHideBasedOnLogin();
    }

}

function showHideBasedOnLogin() {
    if (loggedIn) {
        document.querySelectorAll(".hide-loggedin").forEach(box => { box.classList.add('d-none') });
        document.querySelectorAll(".show-loggedin").forEach(box => { box.classList.remove('d-none') });
        if (Admin) {
            document.querySelectorAll(".admin").forEach(box => { box.classList.remove('d-none') });
        }
        else {
            document.querySelectorAll(".admin").forEach(box => { box.classList.add('d-none') });
        }
    }
    else {
        document.querySelectorAll(".hide-loggedin").forEach(box => { box.classList.remove('d-none') });
        document.querySelectorAll(".show-loggedin").forEach(box => { box.classList.add('d-none') });
        document.querySelectorAll("admin").forEach(box => { box.classList.Add('d-none') });
    }
}


var ready = (callback) => {
    if (document.readyState != "loading") callback();
    else document.addEventListener("DOMContentLoaded", callback);
}

function setLoggedIn(result) { loggedIn = result; }

function setAdmin(result) { Admin = result; }

var loggedIn = false;
var Admin = false;

ready(() => {
    isAdmin();
    isLoggedIn();
});






document.getElementById("signout").addEventListener("click", e => {
    sessionStorage.removeItem('jwt');
    document.location.href = '/';

});

document.getElementById("loginButton").addEventListener("click", e => {
    document.getElementById('loginStatus').innerText = "";
    enableLoginSpinner(true);
    var username = document.getElementById("loginUsername").value;
    var password = document.getElementById("loginPassword").value;
    data = { 'username': username, 'password': password };
    myFetch('/api/v0.999/Login', 'POST', false, data)
        .then(response => {
            if (!response.ok) {
                throw new Error(response.status);
                //return response.json();
            }
        })
        .then(data => {
            //sessionStorage.setItem('jwt', data.token);            
            //document.location.href = '/';
            //isLoggedIn();
            //$('#modalLogin').modal('hide');
            document.getElementById('loginButton').classList.add('d-none');
            document.getElementById('confirmTwoFactorButton').classList.remove('d-none');
            document.getElementById('loginUsernamePasswordContainer').classList.add('d-none');
            document.getElementById('loginTwoFactorFormGroup').classList.remove('d-none');
            enableLoginSpinner(false);
        })
        .catch(error => {
            console.log(error);
            document.getElementById('loginStatus').innerText = "Login Failed";
            enableLoginSpinner(false);
        });
});

document.getElementById("confirmTwoFactorButton").addEventListener("click", e => {
    document.getElementById('loginStatus').innerText = "";
    enableLoginSpinner(true);
    var username = document.getElementById("loginUsername").value;
    var password = document.getElementById("loginPassword").value;
    var code = document.getElementById('loginTwoFactorValue').value;
    data = { 'username': username, 'password': password, 'MultiFactorValue': code };
    myFetch('/api/v0.999/MultiFactor', 'POST', false, data)
        .then(response => {
            if (response.ok) {

                return response.json();
            }
            else
                enableLoginSpinner(false);
            throw new Error(response.status);
        })
        .then(data => {
            sessionStorage.setItem('jwt', data.token);
            document.location.href = '/';
            isLoggedIn();
            //$('#modalLogin').modal('hide');
            //document.getElementById('loginButton').classList.remove('d-none');
            //document.getElementById('confirmTwoFactorButton').classList.add('d-none');
            //document.getElementById('loginUsernamePasswordContainer').classList.remove('d-none');
            //document.getElementById('loginTwoFactorFormGroup').classList.add('d-none');
            enableLoginSpinner(false);
        })
        .catch(error => {
            console.log(error);
            document.getElementById('loginStatus').innerText = "Login Failed";
            enableLoginSpinner(false);
        });
});

// Forgot password

$("#login-forgotpassword").click(function () {
    $("#modalLogin").modal('hide');
    $("#modalForgotPassword").modal('show');
});

document.getElementById("resetPasswordButton").addEventListener("click", e => {
    document.getElementById('resetPasswordStatus').innerText = "";
    enableLoginSpinner(true);
    var email    = document.getElementById("resetPasswordEmail").value;
    var username = document.getElementById("resetPasswordUsername").value;
    var name     = document.getElementById("resetPasswordName").value;
    data = { 'email': email, 'username': username, 'name': name };
    myFetch('/api/v0.999/ForgotPassword', 'POST', false, data)
        .then(response => {
            if (!response.ok) {
                throw new Error(response.status);
            }
        })
        .then(data => {
            document.getElementById('resetPasswordStatus').className = "text-success";
            document.getElementById('resetPasswordStatus').innerText = "Check your email for further instructions";
            enableLoginSpinner(false);
        })
        .catch(error => {
            console.log(error);
            document.getElementById('resetPasswordStatus').className = "text-danger";
            document.getElementById('resetPasswordStatus').innerText = "Password Reset Failed";
            enableLoginSpinner(false);
        });
});


/* Security Questions */

$(function () {
    // On load check for if user is logged in and make request to check to see if they have security questions set up
    var jwt = sessionStorage.getItem('jwt');
    if (jwt != null && jwt !== '') {
        myFetch('/api/v0.999/Internal', 'GET', true)
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.status);
                }
            })
            .catch(exception => { return; }); 

        myFetch('/api/v0.999/SecurityQuestions', 'GET', true)
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.status);
                }
            })
            .catch(exception => { $('#modalSetupSecurityQuestions').modal('show'); }); 
    }
});

// Set up security Questions

document.getElementById("setupSecurityQuestionsButton").addEventListener("click", e => {
    document.getElementById('setupSecurityQuestionsStatus').innerText = "";
    enableLoginSpinner(true);

    var answer1 = document.getElementById("answer1").value;
    var answer2 = document.getElementById("answer2").value;

    data = { 'Answer1': answer1, 'Answer2': answer2};
    myFetch('/api/v0.999/SecurityQuestions/SetupSecurityQuestions', 'POST', true, data)
        .then(response => {
            if (!response.ok) {
                throw new Error(response.status);
            }
        })
        .then(data => {
            document.getElementById('setupSecurityQuestionsStatus').className = "text-success";
            document.getElementById('setupSecurityQuestionsStatus').innerText = "Process successful";
            enableLoginSpinner(false);
            $('#modalSetupSecurityQuestions').modal('hide');
        })
        .catch(error => {
            console.log(error);
            document.getElementById('setupSecurityQuestionsStatus').className = "text-danger";
            document.getElementById('setupSecurityQuestionsStatus').innerText = "Error setting up security questions";
            enableLoginSpinner(false);
        });
});








