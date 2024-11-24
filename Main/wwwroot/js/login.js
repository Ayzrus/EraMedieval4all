document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector(".kt-login-v2__form");

    form.addEventListener("submit", function (event) {
        event.preventDefault();

        const Email = form.email.value.trim();
        const Password = form.password.value.trim();

        if (!Email || !Password) {
            swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Por favor, preencha todos os campos!',
                timer: 3000,
                showConfirmButton: false,
                onOpen: () => {
                    swal.showLoading();
                }
            });
            return;
        }

        const formData = {
            Email,
            Password
        };

        fetch('/Home/Login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(formData)
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }
                throw new Error('Erro na requisição');
            })
            .then(data => {
                if (data.success) {
                    if (data.firstJoin) {
                        swal.fire({
                            title: 'Primeiro login na plataforma mude sua password',
                            input: 'password',
                            inputPlaceholder: 'Digite a password aqui',
                            showCancelButton: true,
                            confirmButtonText: 'Enviar',
                            cancelButtonText: 'Cancelar'
                        }).then((result) => {
                            if (result.value) {
                                if (!validatePassword(result.value)) {
                                    swal.fire({
                                        type: 'error',
                                        title: 'Senha inválida',
                                        text: 'A senha deve ter pelo menos 8 caracteres, incluir pelo 1 letra maiscula e 1 minuscula menos 1 números e 1 caractere especial.',
                                        timer: 3000,
                                        showConfirmButton: false,
                                        onOpen: () => {
                                            swal.showLoading();
                                        }
                                    });
                                    return;
                                }
                                fetch('/Home/UpdatePassword', {
                                    method: 'POST',
                                    headers: {
                                        'Content-Type': 'application/json',
                                    },
                                    body: JSON.stringify({ Password: result.value, Email })
                                })
                                    .then(response => response.json())
                                    .then(data => {
                                        if (data.success) {
                                            swal.fire('Sucesso!', data.message + "make login again with new password!", 'success');
                                        } else {
                                            swal.fire('Erro!', data.message, 'error');
                                        }
                                    })
                                    .catch(error => {
                                        swal.fire('Erro!', 'Houve um problema na comunicação com o servidor.', 'error');
                                    });
                            } else if (result.dismiss) {
                                console.log('Cancelado');
                            }
                        });
                    } else {
                        if (data.cliente == 1) {
                            swal.fire({
                                type: 'success',
                                title: 'Login bem-sucedido',
                                text: 'Você será redirecionado...',
                                timer: 3000,
                                showConfirmButton: false,
                                onOpen: () => {
                                    swal.showLoading();
                                }
                            }).then(() => {

                                window.location.href = '/Users/Perfil';
                            });
                        } else {
                            swal.fire({
                                type: 'success',
                                title: 'Login bem-sucedido',
                                text: 'Você será redirecionado...',
                                timer: 3000,
                                showConfirmButton: false,
                                onOpen: () => {
                                    swal.showLoading();
                                }
                            }).then(() => {

                                window.location.href = '/Dashboard';
                            });
                        }
                        
                    }

                } else {
                    if (data.message == "Account locked.") {
                        swal.fire({
                            type: 'error',
                            title: 'Erro',
                            text: data.message + " Unlock here",
                            timer: 3000,
                            showConfirmButton: false,
                            onOpen: () => {
                                swal.showLoading();
                            }
                        }).then(() => {
                            swal.fire({
                                title: 'Digite seu pin de desbloqueio',
                                input: 'text',
                                inputPlaceholder: 'Digite o pin aqui',
                                showCancelButton: true,
                                confirmButtonText: 'Enviar',
                                cancelButtonText: 'Cancelar'
                            }).then((result) => {
                                if (result.value) {
                                    fetch('/Home/VerifyPin', {
                                        method: 'POST',
                                        headers: {
                                            'Content-Type': 'application/json',
                                        },
                                        body: JSON.stringify({ Pin: result.value, Email })
                                    })
                                        .then(response => response.json())
                                        .then(data => {
                                            if (data.success) {
                                                swal.fire('Sucesso!', "Account unlocked Make login again.", 'success');
                                            } else {
                                                swal.fire('Erro!', data.message, 'error');
                                            }
                                        })
                                        .catch(error => {
                                            swal.fire('Erro!', 'Houve um problema na comunicação com o servidor.', 'error');
                                        });
                                } else if (result.dismiss) {
                                    console.log('Cancelado');
                                }
                            });
                        });

                    } else {

                        swal.fire({
                            type: 'error',
                            title: 'Erro',
                            text: data.message,
                            timer: 3000,
                            showConfirmButton: false,
                            onOpen: () => {
                                swal.showLoading();
                            }
                        });
                    }
                }
            })
            .catch(error => {
                console.error('Erro:', error);
                swal.fire({
                    type: 'error',
                    title: 'Erro na comunicação',
                    text: 'Ocorreu um erro ao tentar fazer login.',
                    timer: 3000,
                    showConfirmButton: false,
                    onOpen: () => {
                        swal.showLoading();
                    }
                });
            });
    });
});


function forgout() {
    swal.fire({
        title: 'Digite seu email',
        input: 'email',
        inputPlaceholder: 'Digite o email aqui',
        showCancelButton: true,
        confirmButtonText: 'Próximo',
        cancelButtonText: 'Cancelar'
    }).then((emailResult) => {
        if (emailResult.value) {
            const email = emailResult.value;

            // Segunda entrada de PIN
            swal.fire({
                title: 'Digite seu pin de desbloqueio',
                input: 'text',
                inputPlaceholder: 'Digite o pin aqui',
                showCancelButton: true,
                confirmButtonText: 'Enviar',
                cancelButtonText: 'Cancelar'
            }).then((pinResult) => {
                if (pinResult.value) {
                    const pin = pinResult.value;

                    fetch('/Home/VerifyPin', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify({ Pin: pin, Email: email })
                    })
                        .then(response => response.json())
                        .then(data => {
                            if (data.success) {
                                swal.fire({
                                    title: 'Digite sua nova senha',
                                    input: 'password',
                                    inputPlaceholder: 'Digite a nova senha aqui',
                                    showCancelButton: true,
                                    confirmButtonText: 'Salvar',
                                    cancelButtonText: 'Cancelar'
                                }).then((passwordResult) => {
                                    if (passwordResult.value) {
                                        const newPassword = passwordResult.value;
                                        if (!validatePassword(newPassword)) {
                                            swal.fire({
                                                type: 'error',
                                                title: 'Senha inválida',
                                                text: 'A senha deve ter pelo menos 8 caracteres, incluir pelo 1 letra maiscula e 1 minuscula menos 1 números e 1 caractere especial.',
                                                timer: 3000,
                                                showConfirmButton: false,
                                                onOpen: () => {
                                                    swal.showLoading();
                                                }
                                            });
                                            return;
                                        }
                                        fetch('/Home/UpdatePassword', {
                                            method: 'POST',
                                            headers: {
                                                'Content-Type': 'application/json',
                                            },
                                            body: JSON.stringify({ Password: newPassword, Email: email })
                                        })
                                            .then(response => response.json())
                                            .then(data => {
                                                if (data.success) {
                                                    swal.fire('Sucesso!', data.message, 'success');
                                                } else {
                                                    swal.fire('Erro!', data.message, 'error');
                                                }
                                            })
                                            .catch(error => {
                                                swal.fire('Erro!', 'Houve um problema na comunicação com o servidor.', 'error');
                                            });
                                    }
                                });
                            } else {
                                swal.fire('Erro!', data.message, 'error');
                            }
                        })
                        .catch(error => {
                            swal.fire('Erro!', 'Houve um problema na comunicação com o servidor.', 'error');
                        });
                } else if (pinResult.dismiss) {
                    console.log('Cancelado');
                }
            });
        } else if (emailResult.dismiss) {
            console.log('Cancelado');
        }
    });
}

function validatePassword(password) {
    const minLength = 8;
    const uppercasePattern = /[A-Z]/;
    const lowercasePattern = /[a-z]/;
    const numberPattern = /[0-9]/;
    const specialCharPattern = /[!@#$%^&*(),.?":{}|<>]/;

    if (password.length < minLength) return false;

    if (!uppercasePattern.test(password)) return false;

    if (!lowercasePattern.test(password)) return false;

    if (!numberPattern.test(password)) return false;

    if (!specialCharPattern.test(password)) return false;

    return true;
}