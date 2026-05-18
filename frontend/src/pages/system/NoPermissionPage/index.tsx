import { ArrowLeft, Lock } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "../../../constants/routes";

export default function NoPermissionPage() {
  const navigate = useNavigate();

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 p-4">
      <div className="max-w-md w-full text-center">    
        <div className="w-32 h-32 bg-white rounded-full flex items-center justify-center mx-auto mb-8 shadow-lg animate-bounce">
          <Lock className="w-16 h-16 text-red-500" />
        </div>

        <h1 className="text-[8rem] font-black text-red-500 opacity-10 leading-none">
          403
        </h1>

        <div className="-mt-8 relative z-10">
          <h2 className="text-3xl font-bold text-red-600 mb-4">
            Acceso Denegado
          </h2>
          <p className="text-gray-600 mb-10">
            Lo sentimos, no tienes los permisos necesarios para acceder a este
            módulo o la sección no tiene opciones habilitadas para tu perfil.
          </p>
        </div>

        <button
          onClick={() => navigate(ROUTES.APP.WORKSPACE)}
          className="inline-flex items-center justify-center gap-2 px-6 py-3 bg-blue-600 text-white font-medium rounded-lg shadow hover:bg-blue-700 transition-colors w-full sm:w-auto"
        >
          <ArrowLeft className="w-5 h-5" />
          Volver al Workspace
        </button>
      </div>
    </div>
  );
}