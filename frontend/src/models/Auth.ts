export interface AuthRequest {
  usuario: string;
  password: string;
}

export interface ModuleData {
  codigoModulo: string;
  nombreModulo: string;
  rutaFrontend?: string | null;
}

export interface LoginResponseData {
  usuarioId: number;
  codigoUsuario: string;
  nombreCompleto: string;
  correo: string;
  empresaId: number | null;
  token: string;
  refreshToken: string;
  requiereMfa: boolean;
  estadoSecretMfa: string;
  perfiles: string[];
  modulos: ModuleData[];
}

export interface AuthResponse {
  rpt: number;
  mensaje: string;
  data: LoginResponseData | null;
  detalleError?: string | null;
}

export interface RefreshRequest {
  refreshToken: string;
}

export interface RefreshResponse {
  rpt: number;
  mensaje: string;
  data: {
    token: string;
    refreshToken: string;
  } | null;
}