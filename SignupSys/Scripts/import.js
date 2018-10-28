$("input[name=\"fileImport\"]").change(function () {
    var fileName = this.value;
    var fileExt = fileName.substring(fileName.lastIndexOf('.')).toLowerCase();
    if (fileExt != ".xls" && fileExt != ".xlsx") {
        alert("所选文件不是 Excel 文件！");
        this.form.reset();
        return false;
    }
    return true;
});