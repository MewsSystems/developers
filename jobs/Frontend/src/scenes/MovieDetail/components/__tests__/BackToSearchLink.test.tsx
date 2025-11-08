import { useRouter } from "next/navigation";
import { render, screen } from "@testing-library/react";
import BackToSearchLink from "@/scenes/MovieDetail/components/BackToSearchLink";
import { userEvent } from "@testing-library/user-event";

jest.mock("next/navigation", () => ({
  useRouter: jest.fn(),
}));
const useRouterMock = useRouter as jest.Mock;

describe("BackToSearchLink", () => {
  it("renders button which calls back", async () => {
    const backMock = jest.fn();
    useRouterMock.mockReturnValue({
      back: backMock,
    });

    render(<BackToSearchLink />);

    const backToSearchButton = screen.getByRole("button", {
      name: "Back to search",
    });
    expect(backToSearchButton).toBeVisible();

    await userEvent.click(backToSearchButton);

    expect(backMock).toHaveBeenCalled();
  });
});
