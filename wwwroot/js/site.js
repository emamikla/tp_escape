function mostrarPista() {
    var pista = document.getElementById("pista");
    if (!pista) return;
    pista.classList.toggle("hidden-form");
}


let palabraSecreta = "JOHNB";
let cantidadIntentos = 0;

function iniciarWordle() 
{
    actualizarContador();
}

function actualizarContador() {
    let contador = document.getElementById("contador");
    if (contador) {
        contador.innerText = `Intento ${cantidadIntentos + 1} de 6`;
    }
}

function comprobarPalabra() {
    let intento = document.getElementById("intento").value.toUpperCase().trim();

    if (intento.length != 5) {
        alert("La palabra debe tener 5 letras.");
        return;
    }

    if (cantidadIntentos >= 6) {
        alert("Ya alcanzaste el máximo de intentos.");
        return;
    }

    let resultado = document.createElement("p");
    resultado.className = "wordle-intento";

    for (let i = 0; i < 5; i++) {
        let letra = document.createElement("span");
        letra.innerText = intento[i];
        letra.className = "wordle-letra";

        if (intento[i] == palabraSecreta[i]) {
            letra.classList.add("correct");
        }
        else if (palabraSecreta.includes(intento[i])) {
            letra.classList.add("partial");
        }
        else {
            letra.classList.add("wrong");
        }

        resultado.appendChild(letra);
    }

    document.getElementById("resultado").appendChild(resultado);
    cantidadIntentos++;
    actualizarContador();
    document.getElementById("intento").value = "";

    console.log("Comparando:", intento, "===", palabraSecreta, "->", intento === palabraSecreta);

    if (intento === palabraSecreta) {
        document.getElementById("intento").disabled = true;
        document.querySelector(".wordle-btn").disabled = true;
        document.getElementById("mensaje-ganador").classList.remove("hidden-message");
        
        setTimeout(function() {
            document.getElementById("respuestaForm").submit();
        }, 1500);
    }
    else if (cantidadIntentos == 6) {
        document.getElementById("intento").disabled = true;
        document.querySelector(".wordle-btn").disabled = true;
        let mensajeError = document.createElement("div");
        mensajeError.className = "wordle-perdido";
        mensajeError.innerHTML = `<h3>Perdiste 😞</h3><p>La palabra era: <strong>${palabraSecreta}</strong></p><p>Recargá la página para intentar de nuevo.</p>`;
        document.getElementById("resultado").appendChild(mensajeError);
    }
}
 