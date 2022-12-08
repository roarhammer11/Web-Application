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

