import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService, LoadingService, UserService } from '@management-books/data-access';
import { finalize } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [MatCardModule, MatInputModule, MatButtonModule, ReactiveFormsModule, RouterModule],
  providers: [AuthService, UserService],
})
export class LoginComponent implements OnInit {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private userService: UserService,
    private loadingService: LoadingService,
    private toastr: ToastrService,
    private route: Router,
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.loadingService.show();
      this.authService
        .authenticate({
          email: this.form.get('email')?.value,
          password: this.form.get('password')?.value,
        })
        .pipe(finalize(() => this.loadingService.hide()))
        .subscribe({
          next: (token) => {
            this.userService.setToken(token);
            this.toastr.success('Autenticado com sucesso!');
            this.route.navigateByUrl('/');
          },
          error: (e) => {
            this.toastr.error(JSON.parse(e.error)[0].message, 'Houve um erro!');
          },
        });
    }
  }

  forgotPasswordSubmit() {
    const email = this.form.get('email');
    if (email?.valid) {
      this.authService.resetPassword(email.value).subscribe((e) => this.toastr.success(e));
    } else {
      this.toastr.error('Forneça um email válido');
    }
  }
}
