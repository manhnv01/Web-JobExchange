function logout() {
    var form = document.getElementById('logoutForm');
    var button = document.getElementById('logout');

    button.addEventListener('click', function (event) {
        event.preventDefault();
        form.submit();
    });
}