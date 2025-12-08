import { ApiResponse } from './apiResponse';

export interface LoginRequest {
  mobile: string;
  password: string;
  code?: string;
}

export interface RegisterRequest {
  mobile: string;
  password: string;
  code: string;
}

export interface LoginResponse {
  accessToken: string;
  expiresIn: number;
  username: string;
}

export interface RegisterResponse {
  username: string;
  expiresIn: number;
}

export interface SendCodeRequest {
  mobile: string;
  isRegister: boolean;
}

export interface SendCodeResponse {
  // Response structure for sending verification code
}

export interface LogoutResponse {
  // Response structure for logout
}

export interface MeResponse {
  id: number;
  mobile: string;
  role: string;
}