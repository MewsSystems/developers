import { Suspense } from "react";

import { Footer } from "./components/Footer";
import { Header } from "./components/Header";
import { LoadingScreen } from "./components/LoadingScreen";
import { ErrorBoundary } from "react-error-boundary";
import { ErrorPage } from "./pages/ErrorPage/ErrorPage.page";

export function AppLayout({ children }: React.PropsWithChildren<unknown>) {
  return (
    <div className="flex min-h-screen flex-col bg-background font-sans">
      <Header />
      <main className="mx-auto w-full max-w-7xl flex-grow">
        <ErrorBoundary fallback={<ErrorPage />}>
          <Suspense fallback={<LoadingScreen className="h-[200px]" />}>
            {children}
          </Suspense>
        </ErrorBoundary>
      </main>
      <Footer />
    </div>
  );
}
