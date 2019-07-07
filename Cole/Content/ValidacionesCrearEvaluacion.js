if (document.getElementById("errorFechaDesde").innerHTML.toString() != "") {

    var textfield = document.getElementById("textfieldFecha");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");
}

if (document.getElementById("errorTitulo").innerHTML.toString() != "") {

    var textfield = document.getElementById("textfieldTitulo");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");

    
}

