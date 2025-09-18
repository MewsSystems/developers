import type { ReactNode } from "react";
import { ErrorBoundary as ReactErrorBoundary } from "react-error-boundary";
import { QueryErrorResetBoundary } from "@tanstack/react-query";
import { ErrorMessage } from "@/shared/ui/molecules/ErrorMessage";

export function ErrorBoundary({ children }: { children: ReactNode }) {
  return (
    <QueryErrorResetBoundary>
      {() => (
        <ReactErrorBoundary FallbackComponent={({ error }: { error: Error }) => <ErrorMessage message={error?.message || "Unexpected error"} />}>
          {children}
        </ReactErrorBoundary>
      )}
    </QueryErrorResetBoundary>
  );
}