import { vi } from "vitest";

vi.mock("@/shared/ui/molecules/EmptyState", () => ({
  EmptyState: () => <div data-testid="empty-state" />
}));
