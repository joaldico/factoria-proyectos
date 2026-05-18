import type {
  AuthRequest,
  AuthResponse,
  RefreshRequest,
  RefreshResponse
} from "../../models/Auth";

export interface AccountRepository {
  authenticate(data: AuthRequest): Promise<AuthResponse>;
  refreshToken(data: RefreshRequest): Promise<RefreshResponse>;
}