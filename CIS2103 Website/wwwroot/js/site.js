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
        getAllDVDs();
        addToCart();
    } else if (title == "Edit DVD") {
        setActive(editDVD);
        const editDVDForm = document.getElementById("edit-dvd-form");
        getAllDVDs();
        setEditDVDForm(editDVDForm);
    } else if (title == "Add DVD") {
        setActive(addDVD);
        const addDVDForm = document.getElementById("add-dvd-form");
        setAddDVDForm(addDVDForm);
    } else if (title == "Cart") {
        setActive(cart);
        setCart();
    } else if (title == "Transactions") {
        setActive(transactions);
    } else if (title == "Accounts") {
        setActive(accounts);
        getAllAccounts();
    }

    /*dashboard eventlisteners*/
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
                        addCashForm.reset();
                        document.getElementById("close-cash-modal-button").click();
                    });
                } else {
                    alert("Server could not process at the moment");
                }
            }).catch((error) => {
                console.log(error);
            }
            );

    });

    /*functions*/

    /*sets active tab*/
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

    /*retrieves accounts for admin*/
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

    function getAllDVDs() {
        const dvds = document.getElementById("dvds");
        if (dvds.hasChildNodes) {
            removeAllChildNodes(dvds);
        }

        fetch("/Home/GetAllDVDs", { method: "GET" })
            .then((response) => {
                if (response.ok) {
                    response.json().then((data) => {
                        const dataLength = Object.keys(data).length;
                        for (var x = 0; x < dataLength; x++) {
                            const container = document.createElement("div");
                            const dvdContainer = document.createElement("a");
                            const dvdId = document.createElement("input");
                            const dvdImage = document.createElement("img");
                            const dvdName = document.createElement("p");
                            const dvdDescription = document.createElement("p");
                            const dvdQuantity = document.createElement("p");
                            const dvdRatePerRent = document.createElement("p");
                            const dvdCategory = document.createElement("p");
                            container.classList.add("d-flex", "flex-column", "flex-wrap");
                            if (data[x].dvdImage != null) {
                                dvdImage.src = data[x].dvdImage;
                            } else {
                                dvdImage.src = "https://via.placeholder.com/210x315";
                            }
                            dvdName.innerHTML = data[x].dvdName;
                            dvdId.innerHTML = data[x].dvdId;
                            dvdQuantity.innerHTML = data[x].quantity;
                            dvdDescription.innerHTML = data[x].description
                            dvdRatePerRent.innerHTML = data[x].ratePerRent;
                            dvdCategory.innerHTML = data[x].category;
                            dvdContainer.href = "#";
                            dvdContainer.setAttribute("onclick", "dynamicallyOpenDVDModal(this)");
                            /*dvdContainer.setAttribute("data-toggle", "modal");
                            dvdContainer.setAttribute("data-target", "dvd-modal");*/
                            dvdId.setAttribute("id", data[x].dvdId);
                            dvdQuantity.setAttribute("id", "dvd-quantity");
                            dvdDescription.setAttribute("id", "dvd-description");
                            dvdRatePerRent.setAttribute("id", "dvd-rate-per-rent");
                            dvdCategory.setAttribute("id", "dvd-category");
                            dvdId.hidden = true;
                            dvdQuantity.hidden = true;
                            dvdDescription.hidden = true;
                            dvdRatePerRent.hidden = true;
                            dvdCategory.hidden = true;
                            dvdContainer.appendChild(dvdId);
                            dvdContainer.appendChild(dvdDescription);
                            dvdContainer.appendChild(dvdQuantity);
                            dvdContainer.appendChild(dvdRatePerRent);
                            dvdContainer.appendChild(dvdCategory);
                            dvdContainer.appendChild(dvdImage);
                            dvdContainer.appendChild(dvdName);
                            container.appendChild(dvdContainer);
                            dvds.appendChild(container);
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

    function getCheckBoxValues() {
        let checkboxes = document.querySelectorAll('input[type="checkbox"]:checked');
        let output = [];
        checkboxes.forEach((checkbox) => {
            output.push(checkbox.value);
        });
        return output.toString();
    }

    function parseDescription(description) {
        return description.replace(/'/g, "''");
    }

    function setAddDVDForm(addDVDForm) {
        addDVDForm.addEventListener('submit', function handleSubmit(event) {
            event.preventDefault();
            const dvdName = $("#DVDName").val().trim();
            const quantity = $("#Quantity").val().trim();
            const category = getCheckBoxValues();
            const description = $("#Description").val().trim();
            const parsedDescription = parseDescription(description);
            const dvdImage = (document.getElementById("Image").files.length != 0) ? document.getElementById("Image").files : "NULL";
            const ratePerRent = $("#RatePerRent").val().trim();
            const formData = new FormData();
            formData.append("DVDName", dvdName);
            formData.append("Quantity", quantity);
            formData.append("Category", category);
            formData.append("Description", parsedDescription);
            formData.append("RatePerRent", ratePerRent);
            if (dvdImage != "NULL") {
                for (const f of dvdImage) {
                    formData.append("DVDImage", f);
                }
            } else {
                formData.append("DVDImage", dvdImage);
            }

            fetch("/Home/AddDVD", { method: "POST", body: formData })
                .then((response) => {
                    if (response.ok) {
                        response.text().then((data) => {
                            alert(data);
                        });
                    } else {
                        alert("Server could not process at the moment");
                    }
                }).catch((error) => {
                    console.log(error);
                }
                );

        });
    }

    function setEditDVDForm(editDVDForm) {
        editDVDForm.addEventListener('submit', function handleSubmit(event) {
            event.preventDefault();
            const closeModalButton = document.getElementById("close-edit-modal-button");
            const dvdId = $("#dvd-id").val().trim();
            const dvdName = $("#dvd-title").val().trim();
            const quantity = $("#dvd-quantity-modal-span").val().trim();
            const category = getCheckBoxValues();
            const description = $("#dvd-description-modal").val().trim();
            const parsedDescription = parseDescription(description);
            const dvdImage = (document.getElementById("Image").files.length != 0) ? document.getElementById("Image").files : "NULL";
            const ratePerRent = $("#dvd-price-modal-span").val().trim();
            const formData = new FormData();
            formData.append("DVDId", dvdId);
            formData.append("DVDName", dvdName);
            formData.append("Quantity", quantity);
            formData.append("Category", category);
            formData.append("Description", parsedDescription);
            formData.append("RatePerRent", ratePerRent);
            if (dvdImage != "NULL") {
                for (const f of dvdImage) {
                    formData.append("DVDImage", f);
                }
            } else {
                formData.append("DVDImage", dvdImage);
            }

            fetch("/Home/EditDVD", { method: "POST", body: formData })
                .then((response) => {
                    if (response.ok) {
                        response.text().then((data) => {
                            alert(data);
                            closeModalButton.click();
                            editDVDForm.reset();
                            getAllDVDs();
                        });
                    } else {
                        alert("Server could not process at the moment");
                    }
                }).catch((error) => {
                    console.log(error);
                }
                );
        });
    }


    function dynamicallyOpenDVDModal(e) {
        /*const dvdModalBody = document.getElementById("modal-body");*/
        const dvdValues = e.querySelectorAll("a > *");
        const dvdCategory = document.getElementById("dvd-category-modal");
        const dvdDescription = document.getElementById("dvd-description-modal");
        const dvdPrice = document.getElementById("dvd-price-modal-span");
        const dvdTitle = document.getElementById("dvd-title");
        const dvdQuantity = document.getElementById("dvd-quantity-modal-span");
        const dvdId = document.getElementById("dvd-id");

        if (title == "Dashboard") {
            const dvdImage = document.getElementById("dvd-image");
            dvdImage.src = dvdValues[5].src;
            dvdDescription.innerHTML = dvdValues[1].innerHTML;
            dvdCategory.innerHTML = dvdValues[4].innerHTML;
            dvdPrice.innerHTML = dvdValues[3].innerHTML;
            dvdTitle.innerHTML = dvdValues[6].innerHTML;
            dvdQuantity.innerHTML = dvdValues[2].innerHTML;
            dvdId.value = parseInt(dvdValues[0].id);
            /*console.log(dvdId.value);*/
            $('#dvd-modal').modal('show');
        } else {
            const categories = dvdValues[4].innerHTML.split(',');
            dvdId.value = dvdValues[0].innerHTML;
            dvdTitle.value = dvdValues[6].innerHTML;
            dvdQuantity.value = dvdValues[2].innerHTML;
            for (var x = 0; x < categories.length; x++) {
                document.getElementById(categories[x]).checked = true;
            }
            dvdDescription.value = dvdValues[1].innerHTML;
            dvdPrice.value = dvdValues[3].innerHTML;
            $('#edit-dvd-modal').modal('show');
        }
    }

    function removeAllChildNodes(parent) {
        while (parent.firstChild) {
            parent.removeChild(parent.firstChild);
        }
    }

    function addToCart() {
        const addToCartButton = document.getElementById("add-to-cart-button");

        addToCartButton.addEventListener("click", function handleSubmit(event) {
            const dvdId = document.getElementById("dvd-id");
            appendCartToSessionStorage("Cart", dvdId.value);
        });
    }

    function appendCartToSessionStorage(name, data) {
        var cart = JSON.parse(sessionStorage.getItem(name));
        const modalCloseButton = document.getElementById("modal-close-button");
        if (cart === null) cart = [];
        if (cart.includes(data) === true) {
            alert("DVD is already in cart");
            modalCloseButton.click();
        } else {
            cart.push(data);
            alert("DVD added to cart");
            modalCloseButton.click();
        }
        sessionStorage.setItem(name, JSON.stringify(cart));
    }

    function setCart() {
        const cartContainer = document.getElementById("cart-container");
        var cart = JSON.parse(sessionStorage.getItem("Cart"));
        if (cart === null) {
            cartContainer.innerHTML = "Cart is empty";
        } else {
            console.log(cart);
        }
    }
}


