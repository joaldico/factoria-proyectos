import { api } from "../../api/axiosClient";
import type {
  AuthRequest,
  AuthResponse,
  RefreshRequest,
  RefreshResponse
} from "../../models/Auth";
import type { AccountRepository } from "./AccountRepository";

export class AccountApi implements AccountRepository {
  
  async authenticate(data: AuthRequest): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>("/Auth/login", data);
    return response.data;
  }

  async refreshToken(data: RefreshRequest): Promise<RefreshResponse> {
    const response = await api.post<RefreshResponse>("/Auth/refresh", data);
    return response.data;
  }
}