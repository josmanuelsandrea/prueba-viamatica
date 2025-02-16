import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserStateService } from './user-state.service';
import { APIResponse, UsuarioDTO } from '../interfaces/general.interfaces';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = 'https://localhost:7149/api/Auth'; // Cambia esta URL según tu backend

    constructor(
        private http: HttpClient,
        private UserStateService: UserStateService
    ) { }

    login(email: string, password: string): Observable<any> {
        return this.http.post(`${this.apiUrl}/login`, { email, password });
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
}
