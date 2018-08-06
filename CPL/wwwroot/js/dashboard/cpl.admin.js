function admin_search(e) {
    var key = e.keyCode || e.which;
    if (key == 13) {
        var searchstring = document.getElementById('search-all-user');
        if (searchstring.value != null && searchstring.value.replace(/\s+/g, '') != "")
            window.location.href = "/Admin/AllUser?search=" + searchstring.value.replace(/\s+/g, '');
    }
}