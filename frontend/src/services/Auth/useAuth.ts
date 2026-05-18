import { useMutation } from "@tanstack/react-query";
import type { AuthRequest, RefreshRequest } from "../../models/Auth";
import { AccountApi } from "./AccountApi";

const accountApi = new AccountApi();

/**
 * Hook mutacional para ejecutar el inicio de sesión corporativo.
 * Proporciona estados reactivos como 'isPending', 'isSuccess' y 'error'.
 */
export const useAuthenticate = () => {
  return useMutation({
    mutationFn: (data: AuthRequest) => accountApi.authenticate(data),
  });
};

/**
 * Hook mutacional para renovar el JWT de forma explícita si fuera necesario
 * (por lo general, el interceptor de Axios se encargará de esto de forma transparente).
 */
export const useRefreshToken = () => {
  return useMutation({
    mutationFn: (data: RefreshRequest) => accountApi.refreshToken(data),
  });
};