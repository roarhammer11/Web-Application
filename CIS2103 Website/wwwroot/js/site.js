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

if (pathName == "/Home/Dashboard") {
    setActive(home);
} else if (pathName == "/Home/Dashboard/EditDVD") {
    setActive(editDVD);
} else if (pathName == "/Home/Dashboard/AddDVD") {
    setActive(addDVD);
} else if (pathName == "/Home/Dashboard/Cart") {
    setActive(cart);
} else if (pathName == "/Home/Dashboard/Transactions") {
    setActive(transactions);
} else if (pathName == "/Home/Dashboard/Accounts") {
    setActive(accounts);
}

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

const form = document.getElementById('signup-form');
const closeModalButton = document.getElementById("close-modal-button");
form.addEventListener('submit', function handleSubmit(event) {
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
        fetch("/Home/AddAccount", { method: "POST", body: formData })
            .then((response) => {
                if (response.ok) {
                    alert("Successfuly Added " + firstName + " " + lastName + " to the database.");
                } else if (response.status == 409) {
                    alert("Account already exists");
                } else {
                    alert("Server could not process at the moment");
                }
            })
            .catch((error) => {
                console.log(error);
            });
        
        closeModalButton.click();
        form.reset();
    }
});


