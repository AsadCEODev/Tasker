// window.toggleTheme = function () {

//     const body = document.body;

//     if (body.classList.contains("dark-layout")) {

//         body.classList.remove("dark-layout");
//         body.classList.add("light-layout");

//         localStorage.setItem("theme", "light");

//     } else {

//         body.classList.remove("light-layout");
//         body.classList.add("dark-layout");

//         localStorage.setItem("theme", "dark");
//     }

//     if (window.feather) {
//         feather.replace();
//     }
// }

// window.loadTheme = function () {

//     const theme = localStorage.getItem("theme");

//     if (theme === "dark") {
//         document.body.classList.add("dark-layout");
//         document.body.classList.remove("light-layout");
//     } else {
//         document.body.classList.remove("dark-layout");
//         document.body.classList.add("light-layout");
//     }

//     if (window.feather) {
//         feather.replace();
//     }
// }

// window.toggleTheme = function (isDark) {

//     const body = document.body;

//     if (isDark) {
//         body.classList.add("dark-layout");
//         body.classList.remove("light-layout");
//         localStorage.setItem("theme", "dark");
//     } else {
//         body.classList.remove("dark-layout");
//         body.classList.add("light-layout");
//         localStorage.setItem("theme", "light");
//     }

//     // Theme icon change
//     const icon = document.querySelector(".nav-link-style .feather");

//     if (icon) {
//         icon.outerHTML = feather.icons[isDark ? "sun" : "moon"].toSvg({
//             class: "ficon"
//         });
//     }
// }

window.toggleTheme = function (isDark) {

    const body = document.body;
    const sidebar = document.querySelector(".main-menu");

    if (isDark) {
        body.classList.add("dark-layout");
        body.classList.remove("light-layout");

        if (sidebar) {
            sidebar.classList.remove("menu-light");
            sidebar.classList.add("menu-dark");
        }

        localStorage.setItem("theme", "dark");
    }
    else {
        body.classList.remove("dark-layout");
        body.classList.add("light-layout");

        if (sidebar) {
            sidebar.classList.remove("menu-dark");
            sidebar.classList.add("menu-light");
        }

        localStorage.setItem("theme", "light");
    }

    feather.replace();
}

window.loadTheme = function () {

    const theme = localStorage.getItem("theme");
    const sidebar = document.querySelector(".main-menu");

    if (theme === "dark") {

        document.body.classList.add("dark-layout");
        document.body.classList.remove("light-layout");

        if (sidebar) {
            sidebar.classList.remove("menu-light");
            sidebar.classList.add("menu-dark");
        }

    } else {

        document.body.classList.remove("dark-layout");
        document.body.classList.add("light-layout");

        if (sidebar) {
            sidebar.classList.remove("menu-dark");
            sidebar.classList.add("menu-light");
        }
    }

    feather.replace();
}

const sidebar = document.querySelector(".main-menu");

if (sidebar) {
    if (isDark) {
        sidebar.classList.remove("menu-light");
        sidebar.classList.add("menu-dark");
    } else {
        sidebar.classList.remove("menu-dark");
        sidebar.classList.add("menu-light");
    }
}