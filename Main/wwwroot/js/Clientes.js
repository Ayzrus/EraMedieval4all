// Function to confirm deletion of an Consult
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
            fetch('/Clientes/Delete', {
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

// Function to handle form submission (save or update athlete)
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
    const url = isEdit === "True" ? '/Clientes/Edit' : '/Clientes/Register';
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
    const telemovelRegex = /^\d{9}$/; // 9 digits, only numbers
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // Valid email pattern

    // Check for required fields
    for (const key in data) {
        if (!data[key]) return 'Preencha todos os campos obrigatórios!';
    }

    // Validate mobile number
    if (!telemovelRegex.test(data.Telefone)) return 'O número de telemóvel deve ter 9 dígitos.';

    // Validate email
    if (!emailRegex.test(data.Email)) return 'Introduza um endereço de correio eletrónico válido.';

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
        Nif: document.getElementById('nif').value,
        Nome: document.getElementById('nome').value,
        Morada: document.getElementById('morada').value,
        Email: document.getElementById('email').value,
        Telefone: document.getElementById('telefone').value,
    };
}

// Function to reset the form
function cancel() {
    document.getElementById('nif').value = '';
    document.getElementById('nome').value = '';
    document.getElementById('morada').value = '';
    document.getElementById('email').value = '';
    document.getElementById('telefone').value = '';
}

// Event listener for input fields to remove non-numeric characters
window.addEventListener('DOMContentLoaded', function () {
    const fieldsToClean = ['telefone'];

    fieldsToClean.forEach(fieldId => {
        const inputField = document.getElementById(fieldId);
        if (inputField) {
            inputField.addEventListener('input', function () {
                this.value = this.value.replace(/\D/g, ''); // Remove non-numeric characters
            });
        }
    });
});
