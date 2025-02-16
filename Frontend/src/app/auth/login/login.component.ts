import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { UserStateService } from 'src/app/services/user-state.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent {
    loginForm: FormGroup;
    errorMessage: string = '';

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private router: Router,
        private userService: UserStateService
    ) {
        this.loginForm = this.fb.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength(6)]]
        });

        this.authService.isAuthenticated().subscribe({
            next: (response) => {
                if (response.data) {
                    this.userService.setUser(response.data);
                    this.router.navigate(['/admin']);
                }
            }
        })
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
                    if (response.user.roles[0].idRolNavigation.nombreRol === 'Administrador') {
                        this.router.navigate(['/admin']);
                    }

                    this.authService.storeToken(response.token);
                    this.router.navigate(['/dashboard']);
                    this.userService.setUser(response.user);
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
