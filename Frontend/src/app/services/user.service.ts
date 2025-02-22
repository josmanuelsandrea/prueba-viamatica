import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { APIResponse, PersonUpdate, SessionHistory, UsuarioDTO } from '../interfaces/general.interfaces';

@Injectable({
    providedIn: 'root'
})
export class UserService {

    private apiUrl = 'https://localhost:7149/api'; // Cambia esta URL según tu backend

    constructor(private http: HttpClient) { }

    getUsers(): Observable<APIResponse<UsuarioDTO[]>> {
        return this.http.get<APIResponse<UsuarioDTO[]>>(`${this.apiUrl}/Usuario/common`);
    }

    getUserById(userId: number): Observable<APIResponse<UsuarioDTO>> {
        return this.http.get<APIResponse<UsuarioDTO>>(`${this.apiUrl}/${userId}`);
    }

    updateUser(data: PersonUpdate): Observable<any> {
        return this.http.put(`${this.apiUrl}/Usuario/updatePerson`, data);
    }

    deactivateUser(userId: number): Observable<any> {
        return this.http.put(`${this.apiUrl}/Usuario/deactivate/${userId}`, null);
    }

    uploadUsers(formData: FormData): Observable<any> {
        return this.http.post(`${this.apiUrl}/upload/xlsx`, formData);
    }
    
    userSessionHistory(userId: number): Observable<APIResponse<SessionHistory[]>> {
        return this.http.get<APIResponse<SessionHistory[]>>(`${this.apiUrl}/Usuario/sessionHistory/${userId}`);
    }

    searchUsersByName(name: string): Observable<APIResponse<UsuarioDTO[]>> {
        return this.http.get<APIResponse<UsuarioDTO[]>>(`${this.apiUrl}/Usuario/search?name=${name}`);
    }
}
