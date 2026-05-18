const ACCESS_TOKEN_KEY = "auth_token_josuecore";
const REFRESH_TOKEN_KEY = "auth_refresh_token_josuecore";
const TEMP_USER = "temp_user_josuecore";
const USER_DATA = "user_data_josuecore";
const CURRENT_MODULE_ID = "current_module_id_josuecore";
const CURRENT_SUCURSAL_CODIGO = "current_sucursal_codigo_josuecore";

export const TokenStorage = {
  setToken: (token: string) => localStorage.setItem(ACCESS_TOKEN_KEY, token),
  getToken: () => localStorage.getItem(ACCESS_TOKEN_KEY),
  clearToken: () => localStorage.removeItem(ACCESS_TOKEN_KEY),

  setRefreshToken: (token: string) =>
    localStorage.setItem(REFRESH_TOKEN_KEY, token),
  getRefreshToken: () => localStorage.getItem(REFRESH_TOKEN_KEY),
  clearRefreshToken: () => localStorage.removeItem(REFRESH_TOKEN_KEY),

  setTempUser: (user: string) => sessionStorage.setItem(TEMP_USER, user),
  getTempUser: () => sessionStorage.getItem(TEMP_USER),

  setUserData: (data: string) => localStorage.setItem(USER_DATA, data),
  getUserData: () => localStorage.getItem(USER_DATA),
  clearUserData: () => localStorage.removeItem(USER_DATA),

  setCurrentModuleId: (id: number) => localStorage.setItem(CURRENT_MODULE_ID, id.toString()),
  getCurrentModuleId: (): number => {
    const id = localStorage.getItem(CURRENT_MODULE_ID);
    return id ? Number(id) : 0;
  },
  clearCurrentModuleId: () => localStorage.removeItem(CURRENT_MODULE_ID),

  setCurrentSucursal: (codigo: string) => 
    localStorage.setItem(CURRENT_SUCURSAL_CODIGO, codigo),
  getCurrentSucursal: (): string | null => 
    localStorage.getItem(CURRENT_SUCURSAL_CODIGO),
  clearCurrentSucursal: () => 
    localStorage.removeItem(CURRENT_SUCURSAL_CODIGO),

  clearSession: () => {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(USER_DATA);
    localStorage.removeItem(CURRENT_MODULE_ID);
    localStorage.removeItem(CURRENT_SUCURSAL_CODIGO);
    sessionStorage.clear();
  },
};
