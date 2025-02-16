import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { SessionHistory, UsuarioDTO } from 'src/app/interfaces/general.interfaces';
import { UserService } from 'src/app/services/user.service';

@Component({
    selector: 'app-session-history-modal',
    templateUrl: './session-history-modal.component.html',
    styleUrls: ['./session-history-modal.component.css']
})
export class SessionHistoryModalComponent {
    @Input() isOpen: boolean = false;
    @Input() sessionHistory: SessionHistory[] = [];
    @Input() user!: UsuarioDTO;
    @Output() close = new EventEmitter<void>();

    constructor(private userService: UserService) {

    }

    ngOnInit(): void {
        this.userService.userSessionHistory(this.user.idUsuario).subscribe({
            next: (response) => {
                this.sessionHistory = response.data;
            },
            error: (error) => {
                console.error('Error al cargar el historial de sesiones', error);
            }
        })
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes['user'] && this.user) {
            this.userService.userSessionHistory(this.user.idUsuario).subscribe({
                next: (response) => {
                    this.sessionHistory = response.data;
                },
                error: (error) => {
                    console.error('Error al cargar el historial de sesiones', error);
                }
            })
        }
    }

    closeModal(): void {
        this.close.emit();
    }
}
