import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginData } from 'src/app/interfaces/auth.interface';
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
            email: ['', [Validators.email]],
            username: [''],
            password: ['', [Validators.required, Validators.minLength(6)]]
        }, { validators: this.atLeastOneFieldRequiredValidator });

        this.authService.isAuthenticated().subscribe({
            next: (response) => {
                if (response.data) {
                    this.userService.setUser(response.data);
                    this.router.navigate(['/admin']);
                }
            }
        });
    }

    atLeastOneFieldRequiredValidator(control: AbstractControl) {
        const email = control.get('email')?.value;
        const username = control.get('username')?.value;
        if (!email && !username) {
            return { atLeastOneRequired: true };
        }
        return null;
    }

    login() {
        if (this.loginForm.invalid) {
            this.errorMessage = 'Debe ingresar al menos un correo electrónico o un nombre de usuario y una contraseña válida';
            return;
        }

        const newLoginData: LoginData = {
            username: this.loginForm.value.username,
            password: this.loginForm.value.password,
            email: this.loginForm.value.email
        };

        this.authService.login(newLoginData).subscribe({
            next: (response) => {
                if (response.token) {
                    if (response.user.roles[0].idRolNavigation.nombreRol === 'Administrador') {
                        this.authService.storeToken(response.token);
                        this.userService.setUser(response.user);
                        this.router.navigate(['/admin']);
                    } else {
                        this.authService.storeToken(response.token);
                        this.userService.setUser(response.user);
                        this.router.navigate(['/dashboard']);
                    }
                } else {
                    this.errorMessage = response.message + '. O posiblemente tu usuario esta deshabilitado.';
                }
            },
            error: (errorResponse) => {
                this.errorMessage = errorResponse.error.message ?? 'Error al iniciar sesión';
                console.error(errorResponse);
            }
        });
    }
}
