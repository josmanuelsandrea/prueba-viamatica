import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { UserStateService } from '../services/user-state.service';
import { map, take } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
    const authService = inject(AuthService);
    const userService = inject(UserStateService);
    const router = inject(Router);

    return authService.isAuthenticated().pipe(
        take(1), // Solo tomamos el primer valor y nos desuscribimos
        map(response => {
            if (response.data) {
                userService.setUser(response.data);
                return true;
            } else {
                router.navigate(['/auth/login']);
                return false;
            }
        })
    );
};
