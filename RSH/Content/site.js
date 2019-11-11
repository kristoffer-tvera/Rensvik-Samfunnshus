var tilDato, fraDato;

function autorun() {

    tilDato = flatpickr("#tilDato", {
        dateFormat: 'Y-m-d',
        altInput: true,
        altFormat: 'd-m-Y',
        minDate: "today",
    });

    fraDato = flatpickr("#fraDato", {
        dateFormat: 'Y-m-d',
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
    var hidden = wrapper.querySelector('input[type="hidden"]');

    var notRequired;
    if (wrapper && wrapper.classList) {
        notRequired = wrapper.classList.toggle('d-none');
    }

    hidden.disabled = notRequired;

    if (notRequired) {
        input.removeAttribute('required');
    } else {
        input.setAttribute('required', 'required');
    }

}

function setDate(year, month, day) {
    var date = new Date(year, month, day);
    fraDato.setDate(date);
}