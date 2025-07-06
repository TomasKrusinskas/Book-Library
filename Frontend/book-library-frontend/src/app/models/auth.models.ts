import { User } from './user.models';

export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface TokenResponse {
  token: string;
  email: string;
  roles: string[];
}

export interface UserProfileResponse {
  user: User;
  token: string;
}
