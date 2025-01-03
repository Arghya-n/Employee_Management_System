export interface AuthState {
  accessToken: string | null;  // The current access token
  refreshToken: string | null; // The current refresh token
  employeeId: number;  // The current authenticated employee's ID
  role: string | null;         // The current authenticated employee's role
}

export interface AuthRequest {
  email: string;
  password: string;
  role: string;
}

export interface AuthResponse {
  token: string;          // The access token for authentication
  refreshToken: string;   // The refresh token for obtaining new tokens           // The role of the authenticated employee (e.g., Admin, User)
  employeeId: number;     // The ID of the authenticated employee
  role: string;
}