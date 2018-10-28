function confirmRecoverAll(notice) {
    if (!notice) {
        notice = "确定要清空全部用户的签到信息吗？";
    }
    return confirm(notice);
}
function confirmRecover(notice) {
    if (!notice) {
        notice = "确定要清空该用户的签到信息吗？";
    }
    return confirm(notice);
}
function confirmDeleteAll(notice) {
    if (!notice) {
        notice = "确定要删除全部记录吗？";
    }
    return confirm(notice);
}
function confirmDelete(notice) {
    if (!notice) {
        notice = "确定要删除该记录吗？";
    }
    return confirm(notice);
}
function btnRecover_onClick() {
    if (confirmRecover()) {
        return __doPostBack('gridSignupList', 'Edit$' + this.id);
    }
    else {
        return false;
    }
}
function btnDelete_onClick() {
    if (confirmDelete()) {
        return __doPostBack('gridSignupList', 'Delete$' + this.id);
    }
    else
    {
        return false;
    }
}
function init() {
    var btns = document.querySelectorAll("div.container input");
    for (var i = 0, j = 0; i < btns.length; i++) {
        var btn = btns[i];
        if (btn.type !== "image") continue;
        if (btn.src.indexOf("recover") != -1) {
            btn.id = j;
            btn.onclick = btnRecover_onClick;
            btn.title = "清空签到信息";
        }
        if (btn.src.indexOf("delete") != -1) {
            btn.id = j;
            btn.onclick = btnDelete_onClick;
            btn.title = "删除记录";
            j++;
        }
    }
}