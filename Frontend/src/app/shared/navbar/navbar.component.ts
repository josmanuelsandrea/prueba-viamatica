import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuOptions } from 'src/app/interfaces/auth.interface';
import { UsuarioDTO } from 'src/app/interfaces/general.interfaces';
import { AuthService } from 'src/app/services/auth.service';
import { UserStateService } from 'src/app/services/user-state.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
    constructor(
        private authService: AuthService,
        private router: Router,
        private userService: UserStateService
        ) { }

    loggedUser: UsuarioDTO | null = null;
    menuOptions: MenuOptions[] = [];


    ngOnInit(): void {
        this.userService.onUserChange().subscribe(user => {
            this.loggedUser = user;
            this.menuOptions = user?.permisos!
            console.log(this.menuOptions);
        })
    }

    redirectToProfile(): void {
        this.router.navigate(['/dashboard']);
    }

    logout(): void {
        this.authService.logout().subscribe({
            next: () => {
                this.authService.deleteToken();
                this.router.navigate(['/login']);        
                this.userService.clearUser();
            },
            error: (error) => {
                alert('Error al cerrar sesión');
                console.error(error);
            }
        }, );
    }
}
