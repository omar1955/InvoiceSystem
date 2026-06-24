export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  email: string;
  fullName: string;
}

export interface RegisterResponse {
  id: string;
  fullName: string;
  email: string;
}