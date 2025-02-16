import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PersonUpdate, UsuarioDTO } from 'src/app/interfaces/general.interfaces';
import { UserService } from 'src/app/services/user.service';

@Component({
    selector: 'app-edit-user-modal',
    templateUrl: './edit-user-modal.component.html',
    styleUrls: ['./edit-user-modal.component.css']
})
export class EditUserModalComponent {
    @Input({ required: true }) isOpen: boolean = false;
    @Input({ required: true }) user!: UsuarioDTO;
    @Output() close = new EventEmitter<void>();
    @Output() userUpdated = new EventEmitter<UsuarioDTO>();

    editUserForm!: FormGroup;

    constructor(private fb: FormBuilder, private userService: UserService) {
        console.log(this.user);
        // this.editUserForm = this.fb.group({
        //     nombres: [this.user.persona?.nombres, Validators.required],
        //     apellidos: [this.user.persona?.apellidos, Validators.required],
        // });

        this.editUserForm = this.fb.group({
            nombres: ['', Validators.required],
            apellidos: ['', Validators.required],
        });
    }

    /** Se activa cada vez que cambia `@Input() user` */
    ngOnChanges(changes: SimpleChanges): void {
        if (changes['user'] && this.user) {
            this.editUserForm.patchValue({
                nombres: this.user.persona?.nombres || '',
                apellidos: this.user.persona?.apellidos || '',
            });
        }
    }

    closeModal(): void {
        this.close.emit();
    }

    saveUser(): void {
        if (this.editUserForm.invalid) return;

        const updateUserData: PersonUpdate = {
            idPersona: this.user.persona?.idPersona || 0,
            nombres: this.editUserForm.value.nombres,
            apellidos: this.editUserForm.value.apellidos
        };

        this.userService.updateUser(updateUserData).subscribe({
            next: (response) => {
                this.userUpdated.emit(response);
                this.closeModal();
            },
            error: (error) => {
                console.error('Error al actualizar el usuario', error);
            }
        });
    }
}
