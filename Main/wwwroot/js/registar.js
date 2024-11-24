document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector(".kt-login-v2__form");
    const telefoneInput = form.telefone;
    const pinInput = form.pin;

    telefoneInput.addEventListener("input", function (event) {
        this.value = this.value.replace(/\D/g, '');
    });

    pinInput.addEventListener("input", function (event) {
        this.value = this.value.replace(/\D/g, '');
    });

    form.addEventListener("submit", function (event) {
        event.preventDefault();

        const Nome = form.nome.value.trim();
        const Morada = form.morada.value.trim();
        const Telefone = form.telefone.value.trim();
        const Email = form.email.value.trim();
        const Password = form.password.value.trim();
        const Pin = form.pin.value.trim();
        const TipoUser = form.tipouser.value.trim();
        const confirmPassword = form.confirm_password.value.trim();

        if (!Nome || !Morada || !Telefone || !Email || !Password || !Pin || !TipoUser || !confirmPassword) {
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

        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(Email)) {
            swal.fire({
                type: 'error',
                title: 'E-mail inválido',
                text: 'Por favor, insira um e-mail válido.',
                timer: 3000,
                showConfirmButton: false,
                onOpen: () => {
                    swal.showLoading();
                }
            });
            return;
        }

        if (Telefone.length !== 9) {
            swal.fire({
                type: 'error',
                title: 'Telefone inválido',
                text: 'O número de telefone deve ter 9 dígitos.',
                timer: 3000,
                showConfirmButton: false,
                onOpen: () => {
                    swal.showLoading();
                }
            });
            return;
        }

        if (Password !== confirmPassword) {
            swal.fire({
                type: 'error',
                title: 'Senhas não coincidem',
                text: 'A senha e a confirmação devem ser iguais.',
                timer: 3000,
                showConfirmButton: false,
                onOpen: () => {
                    swal.showLoading();
                }
            });
            return;
        }

        if (!validatePassword(Password)) {
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

        const formData = {
            Nome,
            Morada,
            Telefone,
            Email,
            Password,
            Pin,
            TipoUser
        };

        fetch('/Home/CreateAccount', {
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
                    swal.fire({
                        type: 'success',
                        title: 'Registro bem-sucedido',
                        text: 'Sua conta foi criada com sucesso!',
                        timer: 3000,
                        showConfirmButton: false,
                        onOpen: () => {
                            swal.showLoading();
                        }
                    }).then(() => {
                        form.reset();
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
            })
            .catch(error => {
                console.error('Erro:', error);
                swal.fire({
                    type: 'error',
                    title: 'Erro na comunicação',
                    text: 'Ocorreu um erro ao tentar se registrar.',
                    timer: 3000,
                    showConfirmButton: false,
                    onOpen: () => {
                        swal.showLoading();
                    }
                });
            });
    });

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
});
