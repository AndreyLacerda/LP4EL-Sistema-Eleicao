CREATE TABLE usuario (
	cod_usuario	NUMERIC(10) PRIMARY KEY,
	email	VARCHAR(100) NOT NULL,
	nome	VARCHAR(100) NOT NULL,
	senha	VARCHAR(160) NOT NULL
);

CREATE SEQUENCE seq_usuario;

create type status_eleicao_t as enum('I','F','P'); --Iniciada(I), Finalizada(F), Preparação(P);

CREATE TABLE eleicao (
	cod_eleicao	NUMERIC(10) PRIMARY KEY,
	titulo	VARCHAR(100) NOT NULL,
	descricao	VARCHAR(500) NOT NULL,
	chave_acesso	VARCHAR(160) NOT NULL,
	voto_multiplo	BOOLEAN NOT NULL,
	status	status_eleicao_t NOT NULL
);

CREATE SEQUENCE seq_eleicao;

CREATE TABLE cargo (
	cod_cargo	NUMERIC(10) PRIMARY KEY,
	cod_eleicao	NUMERIC(10) NOT NULL,
	nome	VARCHAR(50) NOT NULL,
	descricao	VARCHAR(255) NOT NULL,
	quant_votos	INT NOT NULL,
	FOREIGN KEY(cod_eleicao) references eleicao(cod_eleicao)
);

CREATE SEQUENCE seq_cargo;

CREATE TABLE candidato (
	cod_candidato	NUMERIC(10) PRIMARY KEY,
	cod_eleicao	NUMERIC(10) NOT NULL,
	nome	VARCHAR(50) NOT NULL,
	imagem	VARCHAR(255) NOT NULL,
	descricao	VARCHAR(255),
	grupo_partido	VARCHAR(50),
	FOREIGN KEY(cod_eleicao) references eleicao(cod_eleicao)
);

CREATE SEQUENCE seq_candidato;

CREATE TABLE cargo_x_candidato (
    cod_cargo	NUMERIC(10) NOT NULL,
    cod_candidato	NUMERIC(10) NOT NULL,
    cod_eleicao	NUMERIC(10) NOT NULL,
    PRIMARY KEY(cod_cargo, cod_candidato),
    FOREIGN KEY(cod_cargo) references cargo(cod_cargo),
    FOREIGN KEY(cod_candidato) references candidato(cod_candidato),
    FOREIGN KEY(cod_eleicao) references eleicao(cod_eleicao)
);

CREATE TABLE usuario_x_eleicao (
    cod_usuario	NUMERIC(10) NOT NULL,
    cod_eleicao	NUMERIC(10) NOT NULL,
	organizador	boolean NOT NULL,
	voto_concluido	boolean NOT NULL,
    PRIMARY KEY(cod_usuario, cod_eleicao),
    FOREIGN KEY(cod_usuario) references usuario(cod_usuario),
    FOREIGN KEY(cod_eleicao) references eleicao(cod_eleicao)
);

CREATE TABLE voto (
	cod_voto	NUMERIC(10) PRIMARY KEY,
    cod_usuario	NUMERIC(10) NOT NULL,
	cod_cargo	NUMERIC(10) NOT NULL,
    cod_candidato	NUMERIC(10) NOT NULL,
    cod_eleicao	NUMERIC(10) NOT NULL,
    FOREIGN KEY(cod_usuario) references usuario(cod_usuario),
	FOREIGN KEY(cod_eleicao) references eleicao(cod_eleicao),
    FOREIGN KEY(cod_cargo) references cargo(cod_cargo),
    FOREIGN KEY(cod_candidato) references candidato(cod_candidato)
);

CREATE SEQUENCE seq_voto;