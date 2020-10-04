$(document).ready(function () {
    $('#status-modal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var status = button.data('status') // Extract info from data-* attributes
        var id = button.data('id')
        // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
        // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
        var modal = $(this)
        modal.find('.modal-body').text('Você realmente deseja alterar o Status para "' + status + '"?')
        modal.find('a').attr('href', 'PainelEleicao/AlterarStatus?id=' + id + '&status=' + status.charAt(0))
    })

    $('#delete-cargo-modal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var cargoId = button.data('cargoid') // Extract info from data-* attributes
        var id = button.data('id')
        var cargoNome = button.data('cargonome')
        // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
        // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
        var modal = $(this)
        modal.find('.modal-body').text('Você realmente deseja excluir o cargo "' + cargoNome + '" desta eleição?')
        modal.find('a').attr('href', 'Cargo/ExcluirCargo?id=' + id + '&cargo=' + cargoId)
    })

    $('#delete-candidato-modal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var candidatoId = button.data('candidatoid') // Extract info from data-* attributes
        var id = button.data('id')
        var candidatoNome = button.data('candidatonome')
        // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
        // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
        var modal = $(this)
        modal.find('.modal-body').text('Você realmente deseja excluir o candidato "' + candidatoNome + '" desta eleição?')
        modal.find('a').attr('href', 'Candidato/ExcluirCandidato?id=' + id + '&candidato=' + candidatoId)
    })
});