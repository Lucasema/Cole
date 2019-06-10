if (document.getElementById("errorBuscarAño").innerHTML.toString() != ""){

    var textfield = document.getElementById("buscarPorAño");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");
}

if (document.getElementById("errorAñadirAlumno").innerHTML.toString() != "") {

    var textfield = document.getElementById("dniAlumno");
    var textfield2 = document.getElementById("agregarEnAño");

    textfield.classList.add("is-invalid");
    textfield.classList.add("is-dirty");

    textfield2.classList.add("is-invalid");
    textfield2.classList.add("is-dirty");
}