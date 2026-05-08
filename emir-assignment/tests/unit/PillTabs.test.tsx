import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import PillTabs from "../../src/components/PillTabs";
import { describe, it, expect, vi } from "vitest";

const tabs = [
    { key: "overview", label: "Overview" },
    { key: "videos", label: "Videos" },
    { key: "photos", label: "Photos" },
];

describe("PillTabs", () => {
    it("renders all tabs", () => {
        render(<PillTabs tabs={tabs} value="overview" onChange={() => {}} />);
        tabs.forEach((tab) => {
            expect(
                screen.getByRole("tab", { name: tab.label })
            ).toBeInTheDocument();
        });
    });

    it("marks the correct tab as active", () => {
        render(<PillTabs tabs={tabs} value="videos" onChange={() => {}} />);
        const activeTab = screen.getByRole("tab", { name: "Videos" });
        expect(activeTab).toHaveAttribute("aria-selected", "true");

        const inactiveTab = screen.getByRole("tab", { name: "Overview" });
        expect(inactiveTab).toHaveAttribute("aria-selected", "false");
    });

    it("calls onChange with correct key when a tab is clicked", async () => {
        const user = userEvent.setup();
        const onChange = vi.fn();
        render(<PillTabs tabs={tabs} value="overview" onChange={onChange} />);

        await user.click(screen.getByRole("tab", { name: "Photos" }));
        expect(onChange).toHaveBeenCalledWith("photos");
    });

    it("applies custom className to wrapper", () => {
        render(
            <PillTabs
                tabs={tabs}
                value="overview"
                onChange={() => {}}
                className="custom-class"
            />
        );
        expect(screen.getByRole("tablist").className).toMatch(/custom-class/);
    });
});
