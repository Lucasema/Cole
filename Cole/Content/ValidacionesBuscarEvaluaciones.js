if (document.getElementById("errorFechaDesde").innerHTML.toString() != "") {

    var textfield = document.getElementById("textfieldDesde");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");
}

if (document.getElementById("errorFechaHasta").innerHTML.toString() != "") {

    var textfield = document.getElementById("textfieldHasta");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");
}
