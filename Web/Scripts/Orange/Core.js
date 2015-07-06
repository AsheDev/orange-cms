function GetBasePath() {
    var startPos = window.location.pathname.indexOf('/') + 1;
    var endPos = window.location.pathname.indexOf('/', startPos);
    if (endPos < 0) {
        endPos = window.location.pathname.length;
    }
    return '/' + window.location.pathname.substring(startPos, endPos) + '/';
}