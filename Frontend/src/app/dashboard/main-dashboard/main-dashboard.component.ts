import { Component } from '@angular/core';
import { SessionHistory, UsuarioDTO } from 'src/app/interfaces/general.interfaces';
import { UserStateService } from 'src/app/services/user-state.service';
import { UserService } from 'src/app/services/user.service';

@Component({
    selector: 'app-main-dashboard',
    templateUrl: './main-dashboard.component.html',
    styleUrls: ['./main-dashboard.component.css']
})
export class MainDashboardComponent {
    user: UsuarioDTO | null = null;
    isModalOpen: boolean = false;
    sessionHistory: SessionHistory[] = [];

    constructor(
        private userStateService: UserStateService,
        private userService: UserService
    ) { }

    ngOnInit(): void {
        this.loadUserData();
        this.loadSessionHistory();
    }

    loadUserData(): void {
        this.user = this.userStateService.getUser();
    }

    loadSessionHistory(): void {
        this.userService.userSessionHistory(this.user!.idUsuario).subscribe({
            next: (response) => {
                this.sessionHistory = response.data;
            },
            error: (error) => {
                console.error('Error al cargar el historial de sesiones', error);
            }
        })
    }

    editProfile(): void {
        // Aquí puedes redirigir a la página de edición de perfil
        console.log('Editar perfil');
    }

    closeModal(): void {
        this.isModalOpen = false;
    }

    openModal(): void {
        this.isModalOpen = true;
    }

    updateUserList(updatedUser: UsuarioDTO): void {
        window.location.reload();
    }
}
