import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ForgotPasswordRequest } from 'src/app/interfaces/auth.interface';
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: 'app-forgot-password',
    templateUrl: './forgot-password.component.html',
    styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
    forgotPasswordForm: FormGroup;
    successMessage: string = '';
    errorMessage: string = '';
    generatedPassword: string = '';

    constructor(private fb: FormBuilder, private authService: AuthService) {
        this.forgotPasswordForm = this.fb.group({
            email: ['', [Validators.required, Validators.email]]
        });
    }

    resetPassword(): void {
        if (this.forgotPasswordForm.invalid) {
            this.errorMessage = 'Ingrese un correo válido';
            return;
        }

        const forgotPasswordData: ForgotPasswordRequest = {
            email: this.forgotPasswordForm.value.email
        }
        
        this.authService.forgotPassword(forgotPasswordData).subscribe({
            next: (response) => {
                this.generatedPassword = response.data;
            },
            error: (error) => {
                this.errorMessage = 'Error al procesar la solicitud';
                this.successMessage = '';
                console.error(error);
            }
        });
    }
}
