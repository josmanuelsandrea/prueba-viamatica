import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { map, take } from 'rxjs';
import { UserStateService } from '../services/user-state.service';
import { UserService } from '../services/user.service';

export const adminGuard: CanActivateFn = (route, state) => {
    const authService = inject(AuthService);
    const userService = inject(UserStateService);
    const router = inject(Router);

    return authService.isAuthenticated().pipe(
        take(1), // Solo tomamos el primer valor y nos desuscribimos
        map(response => {
            if (response.data) {
                userService.setUser(response.data);
            }

            if (response.data && response.data.roles[0].idRolNavigation.nombreRol === 'Administrador') {
                return true;
            } else {
                router.navigate(['/auth/login']);
                return false;
            }
        })
    );
};
