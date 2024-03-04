function PreventFormSubmition(fromId) {
    document.getElementById(`${formId}`).addEventListener("keydown", function (event) {
        console.log("fromId: ", fromId)

        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
}