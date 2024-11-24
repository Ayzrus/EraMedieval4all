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
            fetch('/Eventos/Delete', {
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
    // Get form values
    const formData = getFormData();

    // Validate form data
    const validationError = validateFormData(formData);
    if (validationError) {
        showError(validationError);
        return;
    }

    // Disable the save button and show loading spinner
    toggleSaveButton(true);

    // Prepare the URL and request data based on edit or register mode
    const url = isEdit === "True" ? '/Eventos/Edit' : '/Eventos/Register';
    const requestData = { ...formData, Id: id };

    // Send data to the server
    fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(requestData)
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
    
    // Regular expressions for each social media platform
    const regexFacebook = /^https?:\/\/(www\.)?facebook\.com\/[A-Za-z0-9_.-]+\/?$/;
    const regexInstagram = /^https?:\/\/(www\.)?instagram\.com\/[A-Za-z0-9_.-]+\/?$/;
    const regexTikTok = /^https?:\/\/(www\.)?tiktok\.com\/@[A-Za-z0-9_.-]+\/?$/;

    // Validate Facebook link
    if (!regexFacebook.test(data.Facebook)) {
        return 'Insira um link válido para o Facebook!';
    }

    // Validate Instagram link
    if (!regexInstagram.test(data.Instagram)) {
        return 'Insira um link válido para o Instagram!';
    }

    // Validate TikTok link
    if (!regexTikTok.test(data.TikTok)) {
        return 'Insira um link válido para o TikTok!';
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
        Localidade: document.getElementById('localidade').value,
        Descricao: document.getElementById('descricao').value,
        Titulo: document.getElementById('titulo').value,
        DataInicio: document.getElementById('datainicio').value,
        DataFim: document.getElementById('datafim').value,
        Facebook: document.getElementById('facebook').value,
        Instagram: document.getElementById('instagram').value,
        TikTok: document.getElementById('tikTok').value,
        Organizador: document.getElementById('organizadores').value,
    };
}

// Function to reset the form
function cancel() {
    document.getElementById('localidade').value = '';
    document.getElementById('descricao').value = '';
    document.getElementById('titulo').value = '';
    document.getElementById('datainicio').value = '';
    document.getElementById('datafim').value = '';
    document.getElementById('facebook').value = '';
    document.getElementById('instagram').value = '';
    document.getElementById('tikTok').value = '';
    document.getElementById('organizadores').value = '';
}

