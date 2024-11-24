// Function to confirm deletion
function confirmDelete(id) {
    // Prepare request data
    const requestData = { Id: id };

    // Show a confirmation dialog
    swal.fire({
        type: "warning",
        title: 'Tem a certeza??',
        text: "Esta ação não pode ser anulada!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim, apaga!',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        // If confirmed, make a POST request to delete the consult
        if (result.value) {
            fetch('/Trajes/Delete', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(requestData)
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        // Show success message and reload the page
                        swal.fire({
                            type: 'success',
                            title: 'Eliminado!',
                            text: 'Eliminado com sucesso.',
                            timer: 3000,
                            showConfirmButton: false,
                            onOpen: () => { swal.showLoading(); }
                        }).then(() => location.reload());
                    } else {
                        // Show error message
                        showError(data.message);
                    }
                })
                .catch((error) => {
                    console.error('Error:', error.message);
                    showError('Ocorreu um erro ao tentar eliminar.');
                });
        }
    });
}

// Utility function to show error messages
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

// Function to handle form submission
function concluir(isEdit, id) {
    // Cria um novo FormData
    const formData = new FormData();

    // Preenche o FormData com os dados do formulário
    const data = getFormData();
    formData.append('Id', id);
    // Adiciona os dados ao FormData
    for (let key in data) {
        formData.append(key, data[key]);
    }

    // Certifique-se de que o arquivo Foto seja enviado
    const fotoInput = document.getElementById('foto');  // Certifique-se de que 'foto' é o id do input de arquivo
    if (fotoInput && fotoInput.files.length > 0) {
        formData.append('Foto', fotoInput.files[0]);
    }

    // Validação de dados (opcional)
    const validationError = validateFormData(data);
    if (validationError) {
        showError(validationError);
        return;
    }

    // Desabilitar botão de salvar e mostrar carregando
    toggleSaveButton(true);

    // URL para a requisição POST
    const url = isEdit === "True" ? '/Trajes/Edit' : '/Trajes/Register';

    // Envia o FormData para o servidor
    fetch(url, {
        method: 'POST',
        body: formData  // Envia o FormData como corpo da requisição
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                swal.fire({
                    type: 'success',
                    title: isEdit === "True" ? 'Atualizado!' : 'Registada!',
                    text: isEdit === "True" ? 'Atualizado com sucesso.' : 'Registada com sucesso.',
                    timer: 3000,
                    showConfirmButton: false,
                    onOpen: () => { swal.showLoading(); }
                }).then(() => location.reload());
            } else {
                showError(data.message);
            }
            // Reset the save button
            toggleSaveButton(false);
        })
        .catch(error => {
            console.error(error.message);
            showError('Ocorreu um erro ao submeter os dados. Por favor, tente novamente.');
            // Reset the save button
            toggleSaveButton(false);
        });
}


// Utility function to validate form data
function validateFormData(data) {
    // Check for required fields
    for (const key in data) {
        if (!data[key]) return 'Preencha todos os campos obrigatórios!';
    }

    return null; // No validation errors
}

// Utility function to toggle the save button state (enable/disable)
function toggleSaveButton(disable) {
    const saveButton = document.getElementById('gravarButton');
    saveButton.classList.toggle("kt-spinner", disable);
    saveButton.classList.toggle("kt-spinner--v2", disable);
    saveButton.classList.toggle("kt-spinner--sm", disable);
    saveButton.classList.toggle("kt-spinner--danger", disable);
    saveButton.innerHTML = disable ? 'Aguarde' : 'Guardar';
    saveButton.disabled = disable;
}

// Utility function to get form data
function getFormData() {
    return {
        Nome: document.getElementById('nome').value,
        Valor: document.getElementById('valor').value,
        Armazem: document.getElementById('armazem').value,
        Foto: document.getElementById('foto').files[0],
        Quantidade: document.getElementById('quantidade').value,
        Especificacao: document.getElementById('especificacao').value,
        Tipo: document.getElementById('tipo').value,
        Ref: document.getElementById('ref').value,
    };
}

// Function to reset the form
function cancel() {
    document.getElementById('nome').value = '';
    document.getElementById('valor').value = '';
    document.getElementById('quantidade').value = '';
    document.getElementById('armazem').value = '';
    document.getElementById('especificacao').value = '';
    document.getElementById('tipo').value = '';
    document.getElementById('foto').value = '';
    document.getElementById('ref').value = '';
}

// Event listener for input fields to remove non-numeric characters
window.addEventListener('DOMContentLoaded', function () {
    const fieldsToClean = ['valor'];

    fieldsToClean.forEach(fieldId => {
        const inputField = document.getElementById(fieldId);
        if (inputField) {
            inputField.addEventListener('input', function () {
                this.value = this.value.replace(/\D/g, ''); // Remove non-numeric characters
            });
        }
    });
});

function off(id) {

    var requestData = { Id: id }

    fetch("/Trajes/Inative", {
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