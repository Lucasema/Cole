if (document.getElementById("errorFechaDesde").innerHTML.toString() != ""){

    var textfield = document.getElementById("fechaDesde");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");
}

if (document.getElementById("errorFechaHasta").innerHTML.toString() != ""){

    var textfield = document.getElementById("fechaHasta");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");
}

if (document.getElementById("errorDni").innerHTML.toString() != "") {

    var textfield = document.getElementById("textDni");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");
}