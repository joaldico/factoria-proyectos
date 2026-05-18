import { ArrowLeft } from "lucide-react";
import { useNavigate } from "react-router-dom";
import josueLogo from "../../../assets/josue.jpg";
import { ROUTES } from "../../../constants/routes";

export default function Header() {
  const navigate = useNavigate();

  return (
    <header className="sticky top-0 z-50 w-full bg-[#0B1120] border-b border-slate-800 py-4 px-6 md:px-10 shadow-lg">
      <div className="w-full flex flex-col md:flex-row items-center justify-between gap-6">
        
        <div className="flex items-center gap-4">
          
          <button
            onClick={() => navigate(ROUTES.APP.WORKSPACE)}
            title="Volver al Workspace"
            className="group flex items-center justify-center w-10 h-10 rounded-xl bg-slate-800/50 border border-slate-700 transition-all duration-300 hover:bg-slate-700 hover:border-cyan-400 hover:shadow-[0_0_10px_rgba(34,211,238,0.2)] active:scale-95"
          >
            <ArrowLeft className="w-5 h-5 text-slate-400 transition-colors group-hover:text-cyan-400" />
          </button>

          <div className="relative p-[2px] rounded-xl bg-gradient-to-r from-cyan-400 to-purple-600 shadow-[0_0_15px_rgba(34,211,238,0.2)]">
            <img
              src={josueLogo}
              alt="Josué Díaz Logo"
              className="w-14 h-14 object-cover rounded-[10px]"
            />
          </div>
          <div className="text-left hidden sm:block">
            <span className="text-transparent bg-clip-text bg-gradient-to-r from-cyan-400 to-purple-400 font-extrabold tracking-widest uppercase text-[10px] mb-0.5 block">
              Software Engineer
            </span>
            <h1 className="text-xl md:text-2xl font-black text-white tracking-tight">
              Josué Díaz
            </h1>
          </div>
        </div>

        <div className="text-center md:text-right">
          <h2 className="text-lg md:text-xl font-bold text-slate-200">
            Proyecto <span className="text-[#FF5A00]">Número 1</span>
          </h2>
          <div className="mt-1 inline-flex items-center gap-2 px-3 py-1 bg-slate-800/50 border border-slate-700 rounded-full">
            <div className="relative flex h-2 w-2">
              <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-cyan-400 opacity-75"></span>
              <span className="relative inline-flex rounded-full h-2 w-2 bg-cyan-500"></span>
            </div>
            <p className="text-xs text-slate-300 font-medium tracking-wide">
              Micro Frontend:{" "}
              <span className="text-white font-bold">Taxímetro</span>
            </p>
          </div>
        </div>

      </div>
    </header>
  );
}