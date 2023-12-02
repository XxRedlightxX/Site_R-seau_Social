function changerCouleurTheme() {
    const currentTheme = document.body.style.backgroundColor;
    const themeButton = document.querySelector(".theme");
    const themeIcon = document.querySelector("#theme-icon");

    if (currentTheme === "rgb(34, 34, 34)" || currentTheme === "") {
        document.body.style.backgroundColor = "#e3d5c3";
        document.querySelector(".container").style.backgroundColor = "#f0e7db";
        const textElements = document.querySelectorAll("h3, p, h2");

        for (let element of textElements) {
            if (!element.classList.contains("titre")) {
                element.style.color = "black";
            }
        }

        const hrElements = document.querySelectorAll("hr");

        for (let hrElement of hrElements) {
            hrElement.style.border = "1px solid #59320b";
        }

        themeIcon.className = "fas fa-moon";
        themeButton.style.backgroundColor = "#29232e";
        themeIcon.style.color = "#e3d5c3";

        localStorage.setItem("theme", "dark");
    } else {
        document.body.style.backgroundColor = "#222222";
        document.querySelector(".container").style.backgroundColor = "#383636";
        const textElements = document.querySelectorAll("h3, p, h2");

        for (let element of textElements) {
            if (!element.classList.contains("titre")) {
                element.style.color = "#EE992C";
            }
        }

        const hrElements = document.querySelectorAll("hr");

        for (let hrElement of hrElements) {
            hrElement.style.border = "none";
        }

        themeButton.style.backgroundColor = "#a1903d";
        themeIcon.className = "fas fa-sun";
        themeIcon.style.color = "#222222";

        localStorage.setItem("theme", "light");
    }
}

const storedTheme = localStorage.getItem("theme");
if (storedTheme === "dark") {
    changerCouleurTheme();
}