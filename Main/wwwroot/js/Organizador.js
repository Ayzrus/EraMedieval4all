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

// Function to handle form submission (save or update athlete)
function concluir() {
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
    const url = '/Eventos/RegisterOrg';
    const requestData = formData;

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
                    title: 'Registada!',
                    text: 'Registada com sucesso.',
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
        Localidade: document.getElementById('localidade').value,
        Nif: document.getElementById('nif').value,
        Tipo: document.getElementById('tipo').value,
    };
}

// Function to reset the form
function cancel() {
    document.getElementById('nome').value = '';
    document.getElementById('localidade').value = '';
    document.getElementById('nif').value = '';
    document.getElementById('tipo').value = '';
}