function mostrarPista() {
    var pista = document.getElementById("pista");
    if (!pista) return;
 
    if (pista.style.display === "none" || pista.style.display === "") {
        pista.style.display = "block";
    } else {
        pista.style.display = "none";
    }
}
 