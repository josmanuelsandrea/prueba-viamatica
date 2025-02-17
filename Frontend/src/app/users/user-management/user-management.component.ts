import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { SessionHistory, UsuarioDTO } from 'src/app/interfaces/general.interfaces';
import { UserService } from 'src/app/services/user.service';

@Component({
    selector: 'app-user-management',
    templateUrl: './user-management.component.html',
    styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent {
    users!: UsuarioDTO[];
    searchQuery: string = '';
    uploadForm: FormGroup;
    selectedFile: File | null = null;
    isModalOpen: boolean = false;
    selectedUser!: UsuarioDTO;
    isHistoryModalOpen: boolean = false; // Estado del modal de historial
    sessionHistory: SessionHistory[] = []; // Lista del historial de sesiones
    filteredUsers: UsuarioDTO[] = [];
    searchControl: FormControl;

    constructor(private fb: FormBuilder, private userService: UserService) {
        this.uploadForm = this.fb.group({
            file: [null, Validators.required]
        });

        this.searchControl = new FormControl('');
    }

    ngOnInit(): void {
        this.loadUsers();

        this.searchControl.valueChanges.pipe(
            debounceTime(300), // Espera 300ms después de que el usuario deje de escribir
            distinctUntilChanged() // Evita llamadas innecesarias si el valor es el mismo
        ).subscribe(query => {
            if (query && query.trim() !== '') {
                this.searchUsers(query);
            } else {
                this.loadUsers(); // Si el campo está vacío, recargar la lista original
            }
        });
    }

    searchUsers(query: string): void {
        this.userService.searchUsersByName(query).subscribe({
            next: (response) => {
                this.filteredUsers = response.data;
            },
            error: (error) => {
                console.error('Error al buscar usuarios', error);
            }
        });
    }

    openHistoryModal(user: UsuarioDTO): void {
        this.selectedUser = user;
        this.isHistoryModalOpen = true;
        // this.loadSessionHistory(user.idUsuario); // Cargar historial del usuario seleccionado
    }

    // 🔹 NUEVO MÉTODO PARA CERRAR EL MODAL DE HISTORIAL
    closeHistoryModal(): void {
        this.isHistoryModalOpen = false;
    }

    closeModal(): void {
        this.isModalOpen = false;
    }

    updateUserList(updatedUser: UsuarioDTO): void {
        window.location.reload();
    }

    openModal(user: UsuarioDTO): void {
        this.selectedUser = user;
        this.isModalOpen = true;
    }

    loadUsers(): void {
        this.userService.getUsers().subscribe({
            next: (response) => {
                this.filteredUsers = response.data;
                this.users = response.data;
            },
            error: (error) => {
                console.error('Error al cargar los usuarios', error);
            }
        });
    }

    deactivateUser(userId: number): void {
        this.userService.deactivateUser(userId).subscribe({
            next: () => {
                this.users = this.users.map(user =>
                    user.idUsuario === userId ? { ...user, status: 'Inactivo' } : user
                );
            },
            error: (error) => {
                console.error('Error al desactivar el usuario', error);
            }
        });
    }

    onFileChange(event: any): void {
        if (event.target.files.length > 0) {
            this.selectedFile = event.target.files[0]; // Almacena el archivo seleccionado
        } else {
            this.selectedFile = null; // Resetea si no hay archivo
        }
    }


    uploadFile(): void {
        if (!this.selectedFile) return;

        const formData = new FormData();
        formData.append('file', this.selectedFile);

        this.userService.uploadUsers(formData).subscribe({
            next: (response) => {
                console.log('Archivo subido correctamente', response);
                this.loadUsers();
                this.selectedFile = null;
                (document.getElementById('file') as HTMLInputElement).value = '';
            },
            error: (error) => {
                console.error('Error al subir el archivo', error);
            }
        });
    }

    onSearchChange(event: any): void {
        const query = event.target.value.toLowerCase();
        this.filteredUsers = this.users.filter(user =>
            user.persona?.nombres.toLowerCase().includes(query) ||
            user.persona?.apellidos.toLowerCase().includes(query) ||
            user.email.toLowerCase().includes(query)
        );
    }
}
