# LP4EL-Sistema-Eleicao
## Projeto de LP4EL
Este projeto consiste em um sistema de eleição. É uma aplicação ASP.NET onde o usuário deve se cadastrar/logar na aplicação para votar ou criar suas próprias eleições. Ao criar uma eleição, o usuário informa detalhes básicos e uma chave de acesso, para que outras pessoas participem da votação. 
No painel de eleição, o administrador registrar Cargos, Candidatos e marca as Candidaturas, relacionando Cargo e Candidato (isso permite que um candidato possa concorrer à mais de um cargo, se necessário). Com tudo cadastrado, o administrador inicia a votação, permitindo que os usuários possam pesquisar a eleição e acessá-la através da chave de acesso. 
Uma vez dentro, o eleitor pode vota, verificar em quem votou e, no final, checar o resultado da apuração. Para a apuração, o adminstrador deve mover o status da elição para "Finalizada". 
A aplicação utiliza banco de dados relacional PostgreSQL, e manipula dados com o EF Core. 
### Integrantes:
* Andrey Camargo Lacerda - SP3013049
* Luis Antonio Goncalves Novaes Angelim - SP301309X
* Pedro Brenicci Freitas - SP3013154
