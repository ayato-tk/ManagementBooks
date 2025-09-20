import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AuthService } from '@management-books/data-access';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
  imports: [MatCardModule, MatInputModule, MatButtonModule, ReactiveFormsModule, RouterModule],
  providers: [AuthService],
})
export class ResetPasswordComponent implements OnInit {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    const token = this.route.snapshot.queryParamMap.get('token');

    if (!token) {
      this.toastr.error('Token inválido ou ausente');
      this.router.navigateByUrl('/auth');
      return;
    }

    this.form = this.fb.group({
      token: [token, Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
    });

  }

  onSubmit() {
    if (this.form.valid) {
      this.authService.resetPassword({ ...this.form.value }).subscribe({
        next: (e) => {
          this.toastr.success(e);
          this.router.navigateByUrl('/auth');
        },
        error: (e) => {
          this.toastr.error(e);
        },
      });
    }
  }
}
