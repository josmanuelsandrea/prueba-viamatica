import { MenuOptions } from "./auth.interface";

export interface APIResponse<T> {
    data:       T;
    message:    string;
    statusCode: number;
}

export interface UsuarioDTO {
    idUsuario:            number;
    intentosInicioSesion: number;
    username:             string;
    password:             string;
    email:                string;
    sessionActive:        string;
    idPersona:            number;
    status:               string;
    eliminado:            boolean;
    persona?: IDPersonaNavigation;
    roles:                Role[];
    permisos:             MenuOptions[];
}

export interface IDPersonaNavigation {
    idPersona:       number;
    nombres:         string;
    apellidos:       string;
    identificacion:  string;
    fechaNacimiento: Date;
    eliminado:       boolean;
}

export interface Role {
    idRolNavigation: IDRolNavigation;
}

export interface IDRolNavigation {
    idRol:     number;
    nombreRol: string;
}

export interface PersonUpdate {
    idPersona: number;
    nombres:   string;
    apellidos: string;
}

export interface SessionHistory {
    idHistorial: number;
    idUsuario:   number;
    fechaInicio: Date;
    fechaCierre: Date;
    exito:       boolean;
    token:       string;
    eliminado:   boolean;
}