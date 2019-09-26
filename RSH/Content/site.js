function autorun() {

    var tilDato = flatpickr("#tilDato", {
        dateFormat: 'Z',
        altInput: true,
        altFormat: 'd-m-Y',
        minDate: "today",
    });

    var fraDato = flatpickr("#fraDato", {
        dateFormat: 'Z',
        altInput: true,
        altFormat: 'd-m-Y',
        minDate: "today",
        onChange: function (selectedDates, dateStr, instance) {
            tilDato.set('minDate', selectedDates[0]);
        }
    });

    $('.flatpicker').on('focus', function () {
        $(this).blur();
    });
    $('.flatpicker').prop('readonly', false);

}
if (document.addEventListener) document.addEventListener("DOMContentLoaded", autorun, false);
else if (document.attachEvent) document.attachEvent("onreadystatechange", autorun);
else window.onload = autorun;

function toggleTilDato() {
    var wrapper = document.querySelector('#tilDatoWrapper');
    var input = wrapper.querySelector('input[type="text"]');

    var notRequired;
    if (wrapper && wrapper.classList) {
        notRequired = wrapper.classList.toggle('d-none');
    }

    if (notRequired) {
        input.removeAttribute('required');
    } else {
        input.setAttribute('required', 'required');
    }

}