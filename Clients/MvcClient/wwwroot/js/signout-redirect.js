window.addEventListener("load", function () {
    var a = document.querySelector("a.PostLogoutRedirectUri");
    console.log(a);
    if (a) {
        window.location = a.href;
    }
});
