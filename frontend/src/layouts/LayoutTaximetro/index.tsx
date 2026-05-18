// src/layouts/LayoutTaximetro.tsx
import { Outlet } from "react-router-dom";
import Footer from "./Footer";
import Header from "./Header";

export default function LayoutTaximetro() {
  return (
    <div className="min-h-screen bg-slate-50 text-slate-800 font-sans flex flex-col selection:bg-indigo-100">
      <Header />

      <main className="flex-grow flex items-center justify-center p-4 sm:p-8 w-full">
        <Outlet />
      </main>

      <Footer />
    </div>
  );
}
