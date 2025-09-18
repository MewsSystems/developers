import { Suspense } from "react";
import type { ReactNode } from "react";
import { Loader } from "@/shared/ui/molecules/Loader";

export function SuspenseBoundary({ children, label = "Loading..." }: { children: ReactNode; label?: string }) {
  return <Suspense fallback={<Loader label={label} />}>{children}</Suspense>;
}
