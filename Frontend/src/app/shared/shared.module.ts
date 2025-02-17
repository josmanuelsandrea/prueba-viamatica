import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { EditUserModalComponent } from './components/edit-user-modal/edit-user-modal.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SessionHistoryModalComponent } from './components/session-history-modal/session-history-modal.component';
import { RouterLink } from '@angular/router';



@NgModule({
  declarations: [
    NavbarComponent,
    EditUserModalComponent,
    SessionHistoryModalComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  exports: [
    EditUserModalComponent,
    SessionHistoryModalComponent,
    NavbarComponent
  ]
})
export class SharedModule { }
