import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UsuarioDTO } from 'src/app/interfaces/general.interfaces';
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

    constructor(private fb: FormBuilder, private userService: UserService) {
        this.uploadForm = this.fb.group({
            file: [null, Validators.required]
        });
    }

    ngOnInit(): void {
        this.loadUsers();
    }

    loadUsers(): void {
        this.userService.getUsers().subscribe({
            next: (response) => {
                this.users = response.data;
                console.log(this.users[0].persona);
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
}
