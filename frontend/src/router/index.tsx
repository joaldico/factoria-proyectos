import React, { Suspense } from "react";
import { createBrowserRouter, Navigate } from "react-router-dom";
import { ROUTES } from "../constants/routes";

import ProtectedRoute from "../components/auth/ProtectedRoute";
import LayoutTaximetro from "../layouts/LayoutTaximetro";
import LoginPage from "../pages/auth/Login";
import GeneralError from "../pages/system/GeneralError";
import NotFoundPage from "../pages/system/NotFound";
import WorkspacePage from "../pages/workspace/WorkspaceSelection";

// Importación dinámica del MFE
const TaximetroApp = React.lazy(() => import("taximetroApp/TaximetroWidget"));

export const router = createBrowserRouter([
  {
    path: "/",
    errorElement: <GeneralError />,
    children: [
      { index: true, element: <Navigate to={ROUTES.AUTH.LOGIN} replace /> },

      { path: ROUTES.AUTH.LOGIN, element: <LoginPage /> },

      {
        element: <ProtectedRoute />,
        children: [
          { path: ROUTES.APP.WORKSPACE, element: <WorkspacePage /> },

          {
            element: <LayoutTaximetro />,
            children: [
              {
                path: "/taximetro/*",
                element: (
                  <Suspense
                    fallback={
                      <div className="flex flex-col items-center justify-center text-blue-600 font-bold animate-pulse w-full">
                        <span className="text-sm">
                          Conectando con el módulo Taxímetro...
                        </span>
                      </div>
                    }
                  >
                    <TaximetroApp />
                  </Suspense>
                ),
              },
            ],
          },
        ],
      },

      { path: "*", element: <NotFoundPage /> },
    ],
  },
]);
