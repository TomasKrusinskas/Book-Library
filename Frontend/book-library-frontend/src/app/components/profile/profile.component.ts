import { Component, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTabsModule } from '@angular/material/tabs';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/user.models';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatTabsModule,
    MatChipsModule,
    MatDividerModule
  ],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  isEditing = false;

  constructor(
    private fb: FormBuilder,
    public authService: AuthService,
    private snackBar: MatSnackBar,
    private router: Router
  ) {
    this.profileForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.refreshUserProfile();
    this.initializeForm();
  }

  get user(): User | null {
    return this.authService.user;
  }

  get isLoggedIn() {
    return this.authService.isLoggedIn;
  }

  get isAdmin() {
    return this.authService.isAdmin;
  }

  private refreshUserProfile(): void {
    console.log('Refreshing user profile...');
    this.authService.loadUserProfile().subscribe({
      next: (user) => {
        console.log('Profile refreshed:', user);
        this.initializeForm();
      },
      error: (error) => {
        console.error('Failed to refresh profile:', error);
        this.snackBar.open('Failed to load profile information', 'Close', { duration: 3000 });
      }
    });
  }

  private initializeForm(): void {
    if (this.user) {
      console.log('Initializing form with user:', this.user);
      this.profileForm.patchValue({
        email: this.user.email,
        firstName: this.user.firstName || '',
        lastName: this.user.lastName || ''
      });
    }
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (!this.isEditing) {
      this.initializeForm(); // Reset form when canceling edit
    }
  }

  async saveProfile(): Promise<void> {
    if (this.profileForm.invalid) {
      this.snackBar.open('Please fill in all required fields correctly', 'Close', { duration: 3000 });
      return;
    }

    try {
      // Here you would typically call an API to update the user profile
      // For now, we'll just show a success message
      this.snackBar.open('Profile updated successfully!', 'Close', { duration: 3000 });
      this.isEditing = false;
    } catch (error) {
      console.error('Error updating profile:', error);
      this.snackBar.open('Failed to update profile', 'Close', { duration: 3000 });
    }
  }

  getMemberSinceDate(): string {
    if (!this.user?.createdAt) return 'N/A';
    return new Date(this.user.createdAt).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  getAccountStatus(): { text: string; color: string } {
    if (!this.user) return { text: 'Unknown', color: 'gray' };
    
    if (this.user.isActive) {
      return { text: 'Active', color: 'green' };
    } else {
      return { text: 'Inactive', color: 'red' };
    }
  }
} 