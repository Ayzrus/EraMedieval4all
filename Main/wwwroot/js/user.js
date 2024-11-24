function showError(message) {
    swal.fire({
        type: 'error',
        title: 'Error',
        text: message,
        timer: 3000,
        showConfirmButton: false,
        onOpen: () => { swal.showLoading(); }
    });
}

function off(id) {

    var requestData = { Id: id }

    fetch("/Users/Inative", {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(requestData)
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                swal.fire({
                    type: 'success',
                    title: 'Inativo!',
                    text: 'Inativado com sucesso.',
                    timer: 3000,
                    showConfirmButton: false,
                    onOpen: () => { swal.showLoading(); }
                }).then(() => location.reload());
            } else {
                showError(data.message);
            }
        })
        .catch(error => {
            console.error(error.message);
            showError('Ocorreu um erro ao submeter os dados. Por favor, tente novamente.');
        });

}