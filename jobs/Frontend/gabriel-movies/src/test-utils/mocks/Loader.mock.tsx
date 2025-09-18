import { vi } from "vitest";

vi.mock("@/shared/ui/molecules/Loader", () => ({
  Loader: ({ label }: { label?: string }) => <div data-testid="loader">{label}</div>
}));