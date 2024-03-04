/*var hoy = new Date();
var dd = hoy.getDate();
var mm = hoy.getMonth() + 1; //Enero es 0
var yyyy = hoy.getFullYear();

if (dd < 10) {
    dd = '0' + dd;
}

if (mm < 10) {
    mm = '0' + mm;
}

hoy = yyyy + '-' + mm + '-' + dd;
document.getElementById("fecha-nacimiento").setAttribute("max", hoy);*/

/*var iframe = document.getElementById('pdfViewer');
iframe.style.width = '800px';
iframe.style.height = '600px';*/


/**** Cambio de input type dependiendo del filtro ****/

var filtro = document.getElementById("filtro-selector");
var buscador_num = document.getElementById("filtro-num");
var buscador_texto = document.getElementById("filtro-texto");
var rango_fechas = document.getElementById("rango-fechas");
var filtro_dep = document.getElementById("filtro-departamento");
document.addEventListener("change", (event) => {
    if (event.target.matches("#filtro-selector")) {
        if (event.target.value == "ID" || event.target.value == "DPI") {
            buscador_num.hidden = false;
            buscador_texto.hidden = true;
            rango_fechas.hidden = true;
            filtro_dep.hidden = true;
        }
        else if (event.target.value == "Nombres") {
            buscador_num.hidden = true;
            buscador_texto.hidden = false;
            rango_fechas.hidden = true;
            filtro_dep.hidden = true;
        }
        else if (event.target.value == "Departamento") {
            console.log(event.target.value);
            buscador_num.hidden = true;
            buscador_texto.hidden = true;
            rango_fechas.hidden = true;
            filtro_dep.hidden = false;
        }
        else if (event.target.value == "Fecha de Ingreso") {
            buscador_num.hidden = true;
            buscador_texto.hidden = true;
            rango_fechas.hidden = false;
            filtro_dep.hidden = true;
        }
    }
});


/*****************************************************/

/************* Cálculo automatico de edad ***************/

/*
var birthdateInput = document.getElementById('fecha-nacimiento');

// Agregar EventListener para realizar calculo de edad.
birthdateInput.addEventListener('input', calculateAge);


function calculateAge() {
    // Extraer fecha del input.
    console.log("funcionando");
    var birthdate = new Date(birthdateInput.value);
    if (!birthdate.getTime()) {
        return; // Exit function if birthdate is invalid
    }

    // Obtener fecha actual.
    var currentDate = new Date();

    //  Calculo de dia, mes, año.
    var age = currentDate.getFullYear() - birthdate.getFullYear();
    var monthDiff = currentDate.getMonth() - birthdate.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && currentDate.getDate() < birthdate.getDate())) {
        age--;
    }

    // Actualizar el input del bloque de edad. Este es readonly.
    var ageInput = document.getElementById('calculo-edad');
    ageInput.value = age;
}
*/

/*****************************************************/
