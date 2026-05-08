import { Meta, StoryObj } from "@storybook/react-vite/*";
import { Main } from "./Main";
import { expect, userEvent, within } from "storybook/internal/test";
import { Route, Routes } from "react-router-dom";

import {
  MovieImagesResponse,
  MovieMockedResponse,
  MoviesMockedResponse,
} from "./mockData";

const handlers = [
  MoviesMockedResponse,
  MovieMockedResponse,
  MovieImagesResponse,
];

const meta = {
  component: Main,
  args: {},
  parameters: {
    msw: {
      handlers,
    },
  },
} satisfies Meta<typeof Main>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default = {
  render: () => (
    <Routes>
      <Route path="/" element={<Main />} />
      <Route path="/:movieId" element={<Main />} />
    </Routes>
  ),
  play: async ({ canvas }) => {
    await expect(await canvas.findByText("Movie search here:")).toBeVisible();

    const search = await canvas.findByRole("textbox");

    userEvent.type(search, "wick");

    await expect(await canvas.findByText("Results:")).toBeVisible();

    const listItems = await canvas.findAllByRole("listitem");

    await userEvent.click(listItems!.at(0)!);

    const movieDetailsPanel = await canvas.findByLabelText(
      "Movie details panel"
    );

    await expect(movieDetailsPanel).toBeVisible();

    await expect(
      await within(movieDetailsPanel).findByRole("definition", {
        name: "Movie Title:",
      })
    ).toHaveTextContent("Wick");

    await expect(
      await within(movieDetailsPanel).findByRole("definition", {
        name: "Release date:",
      })
    ).toHaveTextContent("2023-07-27");

    await expect(
      await within(movieDetailsPanel).findByRole("definition", {
        name: "Runtime:",
      })
    ).toHaveTextContent("2");

    await expect(
      await within(movieDetailsPanel).findByRole("list", {
        name: "Genres",
      })
    ).toHaveTextContent("Animation");

    await expect(
      await within(movieDetailsPanel).findByRole("list", {
        name: "Production Countries",
      })
    ).toHaveTextContent("FranceUnited Kingdom");
  },
} satisfies Story;
