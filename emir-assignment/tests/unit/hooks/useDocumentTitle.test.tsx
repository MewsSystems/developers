import { describe, it, expect, beforeEach } from "vitest";
import { render } from "@testing-library/react";
import { useDocumentTitle } from "../../../src/hooks/useDocumentTitle";

// A helper component to test the hook
function TestComponent({ title }: { title?: string }) {
    useDocumentTitle(title);
    return null;
}

describe("useDocumentTitle", () => {
    beforeEach(() => {
        document.title = "Initial Title"; // reset before each test
    });

    it("sets the document title to the provided title with suffix", () => {
        render(<TestComponent title="Inception" />);
        expect(document.title).toBe("Inception — CinEmir");
    });

    it("sets the document title to default if no title is provided", () => {
        render(<TestComponent />);
        expect(document.title).toBe("CinEmir");
    });

    it("updates the document title when title changes", () => {
        const { rerender } = render(<TestComponent title="One" />);
        expect(document.title).toBe("One — CinEmir");

        rerender(<TestComponent title="Two" />);
        expect(document.title).toBe("Two — CinEmir");
    });
});
