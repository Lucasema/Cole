WindowDatePicker.createLanguage('es', {
    DAYS_ABBR: ['', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sab', 'Dom'],
    MONTHS: ['', 'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
    MONTHS_ABBR: ['', 'Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
    AM_PM: ['AM', 'PM'],
    BUTTONS: ['Aceptar', 'Cancelar'],
    INVALID_DATE: 'Fecha inválida'
});


const picker = new WindowDatePicker({
    el: '#contenedor',
    toggleEl: '#Persona_FechaNacimiento',
    inputEl: '#Persona_FechaNacimiento',
    lang: 'es'
});

picker.el.addEventListener('wdp.change', () => {
    document.getElementById("Persona_FechaNacimiento").parentElement.parentElement.classList.add("is-dirty");

});