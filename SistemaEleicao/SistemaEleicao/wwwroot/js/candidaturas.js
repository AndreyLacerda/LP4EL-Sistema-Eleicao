$(document).ready(function () {
    $('#delete-candidatura-modal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var candidatoId = button.data('candidatoid') // Extract info from data-* attributes
        var id = button.data('id')
        var candidatoNome = button.data('candidatonome')
        var cargoNome = button.data('cargonome')
        var cargoId = button.data('cargoid')
        // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
        // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
        var modal = $(this)
        modal.find('.modal-body').text('Você realmente deseja retirar a candidatura de "' + candidatoNome + '" para o cargo de ' + cargoNome + '?')
        modal.find('a').attr('href', '../Candidatura/ExcluirCandidatura?id=' + id + '&candidato=' + candidatoId + '&cargo=' + cargoId)
    })
});