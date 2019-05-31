if (document.getElementById("errorNombre").innerHTML.toString() != "") {

    var textfield = document.getElementById("nombre");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");
}