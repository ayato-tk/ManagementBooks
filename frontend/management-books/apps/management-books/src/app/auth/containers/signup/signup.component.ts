import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService, LoadingService } from '@management-books/data-access';
import { finalize } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
  imports: [
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    RouterModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
  providers: [MatDatepickerModule, AuthService],
})
export class SignupComponent implements OnInit {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private route: Router,
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      birthDate: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)] ],
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.loadingService.show();
      this.authService
        .signUp({
          ...this.form.value,
          birthDate: (this.form.get('birthDate')?.value as Date).toISOString(),
        })
        .pipe(finalize(() => this.loadingService.hide()))
        .subscribe({
          next: () => {
            this.toastr.success('Usuário cadastrado com sucesso');
            this.route.navigateByUrl('/auth');
          },
          error: (err) => {
            this.toastr.error(err.error[0].message)
          },
        });
    }
  }
}
