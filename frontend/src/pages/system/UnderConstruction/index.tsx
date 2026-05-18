import { ArrowLeft, Construction } from "lucide-react";
import { useNavigate } from "react-router-dom";

export default function UnderConstruction() {
  const navigate = useNavigate();

  return (
    <div className="h-full min-h-screen flex items-center justify-center bg-slate-50 p-4 animate-fade-in">
      <div className="max-w-md w-full">
        <div className="flex flex-col items-center text-center p-8 bg-white rounded-2xl shadow-md border border-slate-200">
          
          <div className="w-20 h-20 rounded-full bg-blue-50 flex items-center justify-center mb-6">
            <Construction className="w-10 h-10 text-blue-500" />
          </div>

          <h1 className="text-2xl font-bold text-slate-900 mb-4">
            En Construcción
          </h1>

          <p className="text-slate-500 mb-8 max-w-[80%] mx-auto">
            Estamos trabajando en este módulo para brindarte la mejor
            experiencia. Pronto estará disponible para la gestión.
          </p>

          <button
            onClick={() => navigate(-1)}
            className="inline-flex items-center gap-2 px-6 py-2.5 rounded-xl border border-slate-200 text-slate-500 font-bold hover:bg-slate-100 hover:border-slate-300 hover:text-slate-900 transition-colors"
          >
            <ArrowLeft className="w-5 h-5" />
            Volver atrás
          </button>
        </div>
      </div>
    </div>
  );
}