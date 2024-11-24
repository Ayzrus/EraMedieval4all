$(function () {
    for (let i = 1; i <= 6; i++) {
        $(`#loading${i}`).show();
    }

    const languageOptions = {
        "sEmptyTable": "Nenhum dado disponível na tabela",
        "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ entradas",
        "sInfoEmpty": "Mostrando 0 até 0 de 0 entradas",
        "sInfoFiltered": "(filtrado de _MAX_ entradas no total)",
        "sLengthMenu": "Mostrar _MENU_ entradas",
        "sLoadingRecords": "A carregar...",
        "sProcessing": "A processar...",
        "sSearch": "Pesquisar:",
        "sZeroRecords": "Nenhum resultado encontrado",
        "oPaginate": {
            "sFirst": "Primeiro",
            "sLast": "Último",
            "sNext": "Próximo",
            "sPrevious": "Anterior"
        },
        "oAria": {
            "sSortAscending": ": ativar para classificar a coluna de forma ascendente",
            "sSortDescending": ": ativar para classificar a coluna de forma descendente"
        }
    };

    const datepickerOptions = {
        format: 'dd/mm/yyyy',
        todayHighlight: true,
        autoclose: true,
        language: 'pt',
        startDate: '01/01/1900',
        endDate: '31/12/2099'
    };

    // Inicializar datepickers
    function initializeDatepickers() {
        $('.datepicker').each(function () {
            $(this).datepicker(datepickerOptions);
        });
    }

    $('.set-today').click(function () {
        const today = new Date();
        const formattedDate = today.toLocaleDateString('pt-BR');
        $('.datepicker').datepicker('setDate', formattedDate);
    });

    $('.clear-date').click(function () {
        $('.datepicker').datepicker('clearDates');
    });

    setTimeout(function () {
        for (let i = 1; i <= 12; i++) {
            $(`#loading${i}`).hide();
            $(`#kt_table_${i}`).show();
        }

        function initializeDataTable(tableId) {
            const noOrderingTables = [6, 7, 8, 9, 10, 11, 12];

            $(`#kt_table_${tableId}`).DataTable({
                scrollY: '400px',
                scrollX: true,
                language: languageOptions,
                ordering: noOrderingTables.includes(tableId) ? false : true
            });
        }

        // Loop para inicializar as tabelas
        for (let i = 1; i <= 12; i++) {
            initializeDataTable(i);
        }


        initializeDatepickers();

    }, 1000);

    if (!document.getElementById('kt_widget_total_orders_chart')) {
        return;
    }

    //// Main chart
    //var max = 80;
    //var color = KTApp.getStateColor('brand');
    //var ctx = document.getElementById('kt_widget_total_orders_chart').getContext("2d");
    //var gradient = ctx.createLinearGradient(0, 0, 0, 120);
    //gradient.addColorStop(0, Chart.helpers.color(color).alpha(0.3).rgbString());
    //gradient.addColorStop(1, Chart.helpers.color(color).alpha(0).rgbString());

    //var data = [30, 35, 45, 65, 35, 50, 40, 60, 35, 45];

    //var mainConfig = {
    //    type: 'line',
    //    data: {
    //        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October'],
    //        datasets: [{
    //            label: 'Orders',
    //            borderColor: color,
    //            borderWidth: 3,
    //            backgroundColor: gradient,
    //            pointBackgroundColor: KTApp.getStateColor('brand'),
    //            data: data,
    //        }]
    //    },
    //    options: {
    //        responsive: true,
    //        maintainAspectRatio: true,
    //        title: {
    //            display: false,
    //            text: 'Stacked Area'
    //        },
    //        tooltips: {
    //            enabled: true,
    //            intersect: false,
    //            mode: 'nearest',
    //            bodySpacing: 5,
    //            yPadding: 10,
    //            xPadding: 10,
    //            caretPadding: 0,
    //            displayColors: false,
    //            backgroundColor: KTApp.getStateColor('brand'),
    //            titleFontColor: '#ffffff',
    //            cornerRadius: 4,
    //            footerSpacing: 0,
    //            titleSpacing: 0
    //        },
    //        legend: {
    //            display: false,
    //            labels: {
    //                usePointStyle: false
    //            }
    //        },
    //        hover: {
    //            mode: 'index'
    //        },
    //        scales: {
    //            xAxes: [{
    //                display: false,
    //                scaleLabel: {
    //                    display: false,
    //                    labelString: 'Month'
    //                },
    //                ticks: {
    //                    display: false,
    //                    beginAtZero: true,
    //                }
    //            }],
    //            yAxes: [{
    //                display: false,
    //                scaleLabel: {
    //                    display: false,
    //                    labelString: 'Value'
    //                },
    //                gridLines: {
    //                    color: '#eef2f9',
    //                    drawBorder: false,
    //                    offsetGridLines: true,
    //                    drawTicks: false
    //                },
    //                ticks: {
    //                    max: max,
    //                    display: false,
    //                    beginAtZero: true
    //                }
    //            }]
    //        },
    //        elements: {
    //            point: {
    //                radius: 0,
    //                borderWidth: 0,
    //                hoverRadius: 0,
    //                hoverBorderWidth: 0
    //            }
    //        },
    //        layout: {
    //            padding: {
    //                left: 0,
    //                right: 0,
    //                top: 0,
    //                bottom: 0
    //            }
    //        }
    //    }
    //};

    //var chart = new Chart(ctx, mainConfig);

    //// Update chart on window resize
    //KTUtil.addResizeHandler(function () {
    //    chart.update();
    //});

});

document.addEventListener('DOMContentLoaded', function () {
    const passwordFields = document.querySelectorAll('.form-group .input-group');

    passwordFields.forEach((group) => {
        const input = group.querySelector('input[type="password"]');
        const toggleIcon = group.querySelector('.input-group-append a i');

        if (input && toggleIcon) {
            group.querySelector('.input-group-append a').addEventListener('click', (event) => {
                event.preventDefault();

                if (input.type === "password") {
                    input.type = "text";
                    toggleIcon.classList.remove("fa-eye");
                    toggleIcon.classList.add("fa-eye-slash");
                } else {
                    input.type = "password";
                    toggleIcon.classList.remove("fa-eye-slash");
                    toggleIcon.classList.add("fa-eye");
                }
            });
        }
    });
});
