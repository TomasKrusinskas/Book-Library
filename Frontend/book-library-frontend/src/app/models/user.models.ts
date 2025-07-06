export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  userName: string;
  roles: string[];
  createdAt: string;
  isActive: boolean;
}

export interface UserStats {
  totalUsers: number;
  adminUsers: number;
  regularUsers: number;
  usersByMonth: { month: string; count: number }[];
  usersByRole: { role: string; count: number }[];
} 