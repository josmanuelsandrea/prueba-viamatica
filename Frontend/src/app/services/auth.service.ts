import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserStateService } from './user-state.service';
import { APIResponse, UsuarioDTO } from '../interfaces/general.interfaces';
import { ForgotPasswordRequest, LoginData } from '../interfaces/auth.interface';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = 'https://localhost:7149/api/Auth'; // Cambia esta URL según tu backend

    constructor(
        private http: HttpClient,
        private UserStateService: UserStateService
    ) { }

    login(data: LoginData): Observable<any> {
        return this.http.post(`${this.apiUrl}/login`, data);
    }

    storeToken(token: string): void {
        localStorage.setItem('auth_token', token);
    }

    getToken(): string | null {
        return localStorage.getItem('auth_token');
    }

    deleteToken(): void {
        localStorage.removeItem('auth_token');
    }

    logout(): Observable<any> {
        return this.http.post(`${this.apiUrl}/logout`, {token: this.getToken()});
    }

    isAuthenticated(): Observable<APIResponse<UsuarioDTO>> {
        return this.http.post<APIResponse<UsuarioDTO>>(`${this.apiUrl}/whoami`, {token: this.getToken()});
    }

    forgotPassword(data: ForgotPasswordRequest): Observable<APIResponse<string>> {
        return this.http.post<APIResponse<string>>(`${this.apiUrl}/forgot-password`, data);
    }
}
