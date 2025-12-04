window.editorCounterValidator = function (textLimit, dotNetRef) {
    const editor = document.getElementById("htmlEditor");
    const limit = textLimit;

    if (!editor) return;

    editor.addEventListener("input", () => {

        let text = editor.value || "";
        console.log("text", text);
        let leftLen = limit - text.length;
        console.log("len", leftLen);
        dotNetRef.invokeMethodAsync("ChangeLimitError", text.length, leftLen);
    });
}


//window.initHtmlEditorCounter = function (dotNetRef) {
//    const editor = document.getElementById("htmlEditor");
//    const limit = 12000;

//    if (!editor) return;

//    editor.addEventListener("input", () => {

//        let text = editor.innerText || "";
//        let len = limit - text.length;
//        console.log("len", len);
//        dotNetRef.invokeMethodAsync("ChangeLimitError", editor.value.length);
//    });
//}