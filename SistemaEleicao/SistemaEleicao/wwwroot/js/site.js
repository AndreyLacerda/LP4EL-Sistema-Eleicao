// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
	// By default submit is disabled 
	$('.botao-busca').prop('disabled', true);

	$('.form-busca').keyup(function () {

		if ($(this).val().length != 0 && !$(this).val().startsWith(" ")) {
			$('.botao-busca').prop('disabled', false);
		} else {
			$(".form-busca").focus();
			$('.botao-busca').prop('disabled', true);
			e.preventDefault();
		}

	});
});