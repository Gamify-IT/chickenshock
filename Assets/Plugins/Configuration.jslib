mergeInto(LibraryManager.library, {
    GetConfiguration: function() {
        var returnStr = window.location.hash.slice(1);
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
});