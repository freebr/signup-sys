function confirmDeleteQuestion(notice) {
    if (!notice) {
        notice = "确定要删除该问题吗？";
    }
    return confirm(notice);
}
function confirmDeleteAnswer(notice) {
    if (!notice) {
        notice = "确定要删除该答案吗？";
    }
    return confirm(notice);
}
function btnDeleteQuestion_onClick() {
    if (confirmDeleteQuestion()) {
        return __doPostBack('gridQuestions', 'Delete$' + this.id);
    }
    else
    {
        return false;
    }
}
function btnDeleteAnswer_onClick() {
    if (confirmDeleteAnswer()) {
        return __doPostBack('gridAnswers', 'Delete$' + this.id);
    }
    else {
        return false;
    }
}
function init() {
    var params = [["td.question-list a", btnDeleteQuestion_onClick, "删除该问题"],
                  ["td.answer-list a", btnDeleteAnswer_onClick, "删除该答案"]]
    for (var k = 0; k < params.length; k++) {
        var links = document.querySelectorAll(params[k][0]);
        for (var i = 0, j = 0; i < links.length; i++) {
            var link = links[i];
            if (link.href.indexOf("Delete") != -1) {
                link.id = j;
                link.onclick = params[k][1];
                link.title = params[k][2];
                j++;
            }
        }
    }
}