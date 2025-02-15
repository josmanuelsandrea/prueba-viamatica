import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent {
    loginForm: FormGroup;
    errorMessage: string = '';

    constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
        this.loginForm = this.fb.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength(6)]]
        });
    }

    login() {
        if (this.loginForm.invalid) {
            this.errorMessage = 'Por favor, complete todos los campos correctamente';
            return;
        }

        const { email, password } = this.loginForm.value;

        this.authService.login(email, password).subscribe({
            next: (response) => {
                if (response.token) {
                    this.authService.storeToken(response.token);
                    this.router.navigate(['/dashboard']);
                } else {
                    this.errorMessage = 'Credenciales incorrectas';
                }
            },
            error: (error) => {
                this.errorMessage = 'Error en la autenticaci n';
            }
        });
    }
}
