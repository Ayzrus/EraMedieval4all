function setModalId(id) {
    document.getElementById('exampleModalCenter').setAttribute('data-id', id);
}

document.querySelector('#exampleModalCenter .salvar').addEventListener('click', function () {
    var modal = document.getElementById('exampleModalCenter');
    var modalId = modal.getAttribute('data-id');
    var dataentrega = document.getElementById('dataentrega').value;
    const requestData = { Id: modalId, DataEntrega: dataentrega };
    fetch("/Aluguer/Copy", {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(requestData)
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                swal.fire({
                    type: 'success',
                    title: 'Copiado!',
                    text: 'Copiado com sucesso.',
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
            // Reset the save button
        });

});

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
            fetch('/Aluguer/Delete', {
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

// Global array to store selected costume IDs
let selectedCostumes = [];

// Function to handle the selection of costumes
function toggleCostumeSelection(costumeId) {
    const index = selectedCostumes.indexOf(costumeId);
    if (index === -1) {
        // Add costume ID to the array if it's not already selected
        selectedCostumes.push(costumeId);
    } else {
        // Remove costume ID from the array if it's already selected
        selectedCostumes.splice(index, 1);
    }

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

    formData.TrajesId = selectedCostumes;

    // Validate form data
    const validationError = validateFormData(formData);
    if (validationError) {
        showError(validationError);
        return;
    }

    // Disable the save button and show loading spinner
    toggleSaveButton(true);

    // Prepare the URL and request data based on edit or register mode
    const url = isEdit === "True" ? '/Aluguer/Edit' : '/Aluguer/Register';
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
        DataEntrega: document.getElementById('dataentrega').value,
        Cliente: document.getElementById('cliente').value,
        Evento: document.getElementById('evento').value,
        TrajesId: [],
    };
}

// Function to reset the form
function cancel() {
    document.getElementById('dataentrega').value = '';
    document.getElementById('cliente').value = '';
    document.getElementById('evento').value = '';

    selectedCostumes = [];
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

function off(id) {

    var requestData = { Id: id }

    fetch("/Aluguer/Finalize", {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(requestData)
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                swal.fire({
                    type: 'success',
                    title: 'Finalizado!',
                    text: 'Finalizado com sucesso.',
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

