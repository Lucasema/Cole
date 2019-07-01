if (document.getElementById("errorCliente").innerHTML.toString() != "") {

    var textfield = document.getElementById("textfieldCliente");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");
}

if (document.getElementById("errorDni").innerHTML.toString() != "") {

    var textfield = document.getElementById("textfieldDni");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");
}