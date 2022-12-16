// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var currentUrl = window.location.href;
var pathName = new URL(currentUrl).pathname;
const home = document.getElementById("Home");
const editDVD = document.getElementById("EditDVD");
const addDVD = document.getElementById("AddDVD");
const cart = document.getElementById("Cart");
const transactions = document.getElementById("Transactions");
const accounts = document.getElementById("Accounts");
var elementArray = [];
elementArray.push(home);
elementArray.push(editDVD);
elementArray.push(addDVD);
elementArray.push(cart);
elementArray.push(transactions);
elementArray.push(accounts);



const signupForm = document.getElementById('signup-form');
const signinForm = document.getElementById("signin-form");
const closeModalButton = document.getElementById("close-modal-button");
if (currentUrl == "https://localhost:7260/") {
    signupForm.addEventListener('submit', function handleSubmit(event) {
        event.preventDefault();
        const firstName = $("#FirstName").val().trim();
        const lastName = $("#LastName").val().trim();
        const email = $("#Email").val().trim();
        const password = $("#Password").val().trim()
        const passwordRepeat = $("#PasswordRepeat").val().trim();
        if (password != passwordRepeat) {
            alert("Password Mismatch");
        } else if (password == passwordRepeat) {
            const formData = new FormData();
            formData.append("FirstName", firstName);
            formData.append("LastName", lastName);
            formData.append("Email", email);
            formData.append("Password", password);
            fetch("/Home/SignUp", { method: "POST", body: formData })
                .then((response) => {
                    if (response.ok) {
                        alert("Successfuly Added " + firstName + " " + lastName + " to the database.");
                        closeModalButton.click();
                        signupForm.reset();
                    } else if (response.status == 409) {
                        alert("Email already exists");
                    } else {
                        alert("Server could not process at the moment");
                    }
                }).catch((error) => {
                    console.log(error);
                }
                );
        }
    });

    signinForm.addEventListener('submit', function handleSubmit(event) {
        event.preventDefault();
        const email = $("#EmailSignIn").val().trim();
        const password = $("#PasswordSignIn").val().trim()
        const formData = new FormData();
        formData.append("Email", email);
        formData.append("Password", password);
        fetch("/Home/SignIn", { method: "POST", body: formData })
            .then((response) => {
                if (response.ok) {
                    response.json().then((data) => {
                        /*accountId = data.accountId;*/
                        window.location.href = "Home/Dashboard?accountId=" + data.accountId;
                    });
                    signinForm.reset();
                } else if (response.status == 401) {
                    alert("Invalid Credentials");
                } else {
                    alert("Server could not process at the moment");
                }
            }).catch((error) => {
                console.log(error);
            }
            );
    });
} else {
    const accountUpdateCredentialsForm = document.getElementById("account-settings-form");
    const accountDeleteForm = document.getElementById("account-settings-form-delete");
    const addCashForm = document.getElementById("add-cash-form");
    const closeModalButton = document.getElementById("close-modal-button");
    if (privilege == "User") {
        editDVD.style.display = "none";
        addDVD.style.display = "none";
        accounts.style.display = "none";
    } else if (privilege == "Staff") {
        accounts.style.display = "none";
    }

    if (title == "Dashboard") {
        setActive(home);
    } else if (title == "Edit DVD") {
        setActive(editDVD);
    } else if (title == "Add DVD") {
        setActive(addDVD);
    } else if (title == "Cart") {
        setActive(cart);
    } else if (title == "Transactions") {
        setActive(transactions);
    } else if (title == "Accounts") {
        setActive(accounts);
        getAllAccounts();
    }

    accountUpdateCredentialsForm.addEventListener('submit', function handleSubmit(event) {
        event.preventDefault();
        const oldEmail = $("#OldEmail").val().trim();
        const newEmail = $("#NewEmail").val().trim();
        const oldPassword = $("#OldPassword").val().trim()
        const newPassword = $("#NewPassword").val().trim();
        const newPasswordRepeat = $("#NewPasswordRepeat").val().trim()
        if (newPassword == newPasswordRepeat) {
            const formData = new FormData();
            formData.append("OldEmail", oldEmail);
            formData.append("NewEmail", newEmail);
            formData.append("OldPassword", oldPassword);
            formData.append("NewPassword", newPassword);
            fetch("/Home/UpdateAccountCredentials", { method: "POST", body: formData })
                .then((response) => {
                    if (response.ok) {
                        response.text().then((data) => {
                            alert(data);
                            closeModalButton.click();
                            accountUpdateCredentialsForm.reset();
                        });
                    } else if (response.status == 401) {
                        alert("Invalid Credentials");
                    } else {
                        alert("Server could not process at the moment");
                    }
                }).catch((error) => {
                    console.log(error);
                }
                );
        } else {
            alert("Password Mismatch");
        }
    });

    accountDeleteForm.addEventListener('submit', function handleSubmit(event) {
        event.preventDefault();
        const email = $("#OldEmail").val().trim();
        const deleteInput = $("#DeleteAccount").val().trim()
        if (deleteInput == "DELETE") {
            const formData = new FormData();
            formData.append("Email", email);

            fetch("/Home/DeleteAccount", { method: "POST", body: formData })
                .then((response) => {
                    if (response.ok) {
                        response.text().then((data) => {
                            alert(data);
                            window.location.href = "/";
                        });
                    } else {
                        alert("Server could not process at the moment");
                    }
                }).catch((error) => {
                    console.log(error);
                }
                );
        } else {
            alert("Please input DELETE to proceed");
        }
    });

    addCashForm.addEventListener('submit', function handleSubmit(event) {
        event.preventDefault();
        const cashAmount = $("#CashAmount").val().trim();
        const accountId = $("#AccountId").val().trim();
        const cashLabel = document.getElementById("cash-label");
        const formData = new FormData();
        formData.append("CashAmount", cashAmount);
        formData.append("AccountId", accountId);
        fetch("/Home/AddCash", { method: "POST", body: formData })
            .then((response) => {
                if (response.ok) {
                    response.json().then((data) => {
                        alert(data.message);
                        cashLabel.innerHTML = "Cash : " + data.cash;
                    });
                } else {
                    alert("Server could not process at the moment");
                }
            }).catch((error) => {
                console.log(error);
            }
            );

    });

    function setActive(element) {
        const index = elementArray.indexOf(element);
        elementArray[index].classList.add("active");
        elementArray.splice(index, 1);
        for (var x = 0; x < 4; x++) {
            if (elementArray[x].classList.contains("active")) {
                elementArray[x].classList.remove("active");
            }
        }
    }

    function getAllAccounts() {
        const accountsTable = document.getElementById("accountsTable");
        fetch("/Home/GetAllAccounts", { method: "GET" })
            .then((response) => {
                if (response.ok) {
                    response.json().then((data) => {
                        const dataLength = Object.keys(data).length;
                        for (var x = 0; x < dataLength; x++) {
                            let tableRow = document.createElement("tr");
                            let firstName = document.createElement("td");
                            let lastName = document.createElement("td");
                            let status = document.createElement("td");
                            let privilege = document.createElement("td");
                            let actions = document.createElement("td");
                            firstName.innerHTML = data[x].firstName;
                            lastName.innerHTML = data[x].lastName;
                            status.innerHTML = data[x].status;
                            privilege.innerHTML = data[x].privilege;
                            actions.innerHTML = "Edit";
                            tableRow.appendChild(firstName);
                            tableRow.appendChild(lastName);
                            tableRow.appendChild(status);
                            tableRow.appendChild(privilege);
                            tableRow.appendChild(actions);
                            accountsTable.appendChild(tableRow);

                        }
                    });
                } else {
                    alert("Server could not process at the moment");
                }
            }).catch((error) => {
                console.log(error);
            }
            );
    }
}


