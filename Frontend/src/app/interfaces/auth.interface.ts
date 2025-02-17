export interface LoginData {
    username: string;
    password: string;
    email: string;
}

export interface ForgotPasswordRequest {
    email: string;
}

export interface MenuOptions {
    nombreOpcion: string;
    url: string;
}