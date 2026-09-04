// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function mostrarPista() {
    const pista = document.getElementById('pista');
    const boton = document.getElementById('mostrarPista');

    if (pista && boton) {
        if (pista.style.display === 'none') {
            pista.style.display = 'block';
            boton.textContent = 'Ocultar pista';
        } else {
            pista.style.display = 'none';
            boton.textContent = 'Mostrar pista';
        }
    }
}
