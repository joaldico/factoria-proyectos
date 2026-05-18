import { Component, Power } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "../../../constants/routes";
import type { LoginResponseData, ModuleData } from "../../../models/Auth";
import { TokenStorage } from "../../../services/General/Storage/TokenStorage";

export default function WorkspacePage() {
  const navigate = useNavigate();
  
  const userData = TokenStorage.getUserData();
  const user: LoginResponseData | null = userData ? JSON.parse(userData) : null;
  
  if (!user) {
    navigate(ROUTES.AUTH.LOGIN);
    return null;
  }

  const modules = user.modulos || [];

  const handleLogout = () => {
    TokenStorage.clearSession();
    navigate(ROUTES.AUTH.LOGIN);
  };

  const handleModuleClick = (mod: ModuleData) => {
    if (mod.rutaFrontend && mod.rutaFrontend.trim() !== "") {
      navigate(mod.rutaFrontend);
    } else {
      alert(`El módulo ${mod.nombreModulo} está en construcción o no tiene ruta.`);
    }
  };

  const renderMainComponent = () => {
    if (modules.length === 0) {
      return (
        <p className="text-slate-500 text-xl font-medium text-center mt-10">
          No tienes entornos de trabajo asignados. Contacta al administrador.
        </p>
      );
    }

    return (
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6 w-full max-w-4xl mx-auto mt-10 animate-[fadeIn_0.8s_ease-out_forwards]">
        {modules.map((mod: ModuleData) => {
          const baseColor = "#3b82f6";

          return (
            <div
              key={mod.codigoModulo}
              onClick={() => handleModuleClick(mod)}
              className="relative overflow-hidden bg-white border border-slate-100 p-6 rounded-[2rem] flex items-center gap-6 cursor-pointer transition-all duration-300 hover:bg-blue-50/30 hover:-translate-y-1 hover:shadow-xl group shadow-sm"
            >
              <div
                className="absolute left-0 top-0 w-1.5 h-full transition-colors"
                style={{ backgroundColor: baseColor }}
              />

              <div
                className="w-16 h-16 rounded-2xl flex items-center justify-center text-white shrink-0 transition-transform duration-300 group-hover:scale-110"
                style={{
                  backgroundColor: baseColor,
                  boxShadow: `0 10px 15px -3px ${baseColor}40`,
                }}
              >
                <Component className="w-8 h-8" />
              </div>

              <div className="text-left">
                <h3 className="text-lg font-black text-slate-900 uppercase tracking-tight mb-1">
                  {mod.nombreModulo}
                </h3>
                <p className="text-slate-500 text-xs font-medium leading-tight">
                  Acceso al entorno de trabajo
                </p>
              </div>
            </div>
          );
        })}
      </div>
    );
  };

  return (
    <div className="min-h-screen w-full flex flex-col items-center justify-center bg-slate-50 p-6 relative font-sans">
      
      <div className="absolute top-8 right-8 md:right-10 flex items-center gap-4 z-10">
        <div
          className="flex items-center gap-3 cursor-pointer text-slate-500 hover:text-red-500 transition-colors duration-300 group"
          onClick={handleLogout}
        >
          <span className="text-[10px] font-black uppercase tracking-widest opacity-0 group-hover:opacity-100 transition-opacity duration-300">
            Cerrar Sesión
          </span>
          <button className="w-12 h-12 rounded-2xl bg-white border border-slate-200 flex items-center justify-center group-hover:border-red-500 group-hover:shadow-md transition-all duration-300 text-inherit">
            <Power className="w-6 h-6" />
          </button>
        </div>
      </div>

      <div className="w-full max-w-4xl text-center relative z-10">
        
        <div className="mb-10 animate-[fadeIn_0.8s_ease-out_forwards]">
          <div className="inline-block relative mb-4">
            <div className="w-32 h-32 bg-white rounded-full flex items-center justify-center shadow-[0_15px_35px_-5px_rgba(0,0,0,0.05)] border border-blue-100 p-1 mb-2 transition-transform duration-500 hover:scale-105">
              <img
                src="/josue.jpg"
                alt="Josué Díaz"
                className="w-full h-full object-cover rounded-full"
                onError={(e) => {
                  e.currentTarget.src = "/josue.jpg";
                }}
              />
            </div>
          </div>
          
          <p className="text-blue-600 font-bold text-xs tracking-widest uppercase mb-3">
            Factoría Proyectos Suite
          </p>
          <h1 className="text-4xl md:text-[2.25rem] font-black text-slate-900 uppercase tracking-tighter leading-none mb-3">
            BIENVENIDO, {user.nombreCompleto.toUpperCase()}
          </h1>
          <p className="text-slate-500 font-medium tracking-wide text-sm">
            Selecciona un entorno de trabajo
          </p>
        </div>

        {renderMainComponent()}

        <div className="mt-16 opacity-60 text-center">
          <p className="text-slate-400 text-[10px] font-black uppercase tracking-[0.4em]">
            POWERED BY JOSUÉ DÍAZ • FACTORÍA PROYECTOS SUITE V1.0
          </p>
        </div>
      </div>
    </div>
  );
}