//declare namespace to avoid collisions with extension js in the browser
var langGlobalNamespace = {};

langGlobalNamespace.switchLanguage = function(lang) {
    $.cookie('language', lang, { expires: 365, path: '/' });
    window.location.reload();
};

$(function () {
    $("#setRus").click(function () {
        langGlobalNamespace.switchLanguage('ru');
    });
    $("#setEng").click(function () {
        langGlobalNamespace.switchLanguage('en');
    });
});