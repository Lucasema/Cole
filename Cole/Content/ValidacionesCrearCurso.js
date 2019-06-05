if (document.getElementById("errorNro").innerHTML.toString() != "") {

    var textfield = document.getElementById("nro");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");
}

if (document.getElementById("errorDivision").innerHTML.toString() != "") {

    var textfield = document.getElementById("division");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");
}

if (document.getElementById("errorDuplicado").innerHTML.toString() != "") {

    var textfield1 = document.getElementById("nro");
    var textfield2 = document.getElementById("division");

    textfield1.classList.add("is-invalid");
    textfield1.classList.add("is-dirty");

    textfield2.classList.add("is-invalid");
    textfield2.classList.add("is-dirty");
}
