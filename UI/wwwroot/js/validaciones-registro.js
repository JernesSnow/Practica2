$(function () {

    function validarNombre() {
        const value = $("#NombreCompleto").val();
        const regex = /^[A-Za-zÁÉÍÓÚÑáéíóúñ\s]{3,}$/;

        if (!regex.test(value)) {
            $("#errNombre").text("Mínimo 3 letras, sin números.");
            return false;
        } else {
            $("#errNombre").text("");
            return true;
        }
    }

    function validarCorreo() {
        const value = $("#Correo").val();
        const regex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;

        if (!regex.test(value)) {
            $("#errCorreo").text("Debe ser un correo válido con @ y .");
            return false;
        } else {
            $("#errCorreo").text("");
            return true;
        }
    }

    function validarTelefono() {
        const value = $("#Telefono").val();
        const regex = /^\d{9,}$/;

        if (!regex.test(value)) {
            $("#errTelefono").text("Solo números y mínimo 9 dígitos.");
            return false;
        } else {
            $("#errTelefono").text("");
            return true;
        }
    }

    $("#NombreCompleto").on("input blur", validarNombre);
    $("#Correo").on("input blur", validarCorreo);
    $("#Telefono").on("input blur", validarTelefono);

    $("#formRegistro").on("submit", function (e) {
        const okNombre = validarNombre();
        const okCorreo = validarCorreo();
        const okTelefono = validarTelefono();

        if (!okNombre || !okCorreo || !okTelefono) {
            e.preventDefault();
        }
    });
});
