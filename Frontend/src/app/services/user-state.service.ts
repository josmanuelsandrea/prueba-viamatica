import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { UsuarioDTO } from '../interfaces/general.interfaces';

@Injectable({
    providedIn: 'root'
})
export class UserStateService {
    private userSubject = new BehaviorSubject<UsuarioDTO | null>(null);
    user$ = this.userSubject.asObservable(); // Observable público para suscribirse

    constructor() { }

    /** Establece el usuario actual */
    setUser(user: UsuarioDTO): void {
        this.userSubject.next(user);
    }

    /** Obtiene el usuario actual */
    getUser(): UsuarioDTO | null {
        return this.userSubject.getValue();
    }

    /** Limpia el usuario almacenado */
    clearUser(): void {
        this.userSubject.next(null);
    }

    /** Detecta cambios en el usuario en tiempo real */
    onUserChange(): Observable<UsuarioDTO | null> {
        return this.user$; // Devuelve el observable al que los componentes pueden suscribirse
    }
}
