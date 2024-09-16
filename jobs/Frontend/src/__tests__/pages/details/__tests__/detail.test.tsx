import { render, screen } from "@testing-library/react";

jest.mock("next/image", () => (props: any) => <span>{props.src}</span>);

import { Movie, MovieSummary, TestReq, TestRes } from "@/types";
import Detail, { getServerSideProps } from "../../../../pages/detail/[id]";
import { IncomingMessage, ServerResponse } from "http";
import { NextApiRequestCookies } from "next/dist/server/api-utils";
import { movieService } from "@/services/movieService";

jest.mock("../../../../services/movieService");

const defaultMovie: Movie & {
  [key: string]: string | string[] | number | undefined;
} = {
  id: 1,
  title: "Movie title",
  overview: "Overview summary text",
  posterImage: "image",
  genres: ["Romance", "Comedy"],
  homepage: "https://net.com",
  spokenLanguages: ["Italian"],
};

describe("Detail page", () => {
  it("should show detail page", () => {
    render(<Detail {...defaultMovie} />);
    const actual = screen.getByText("Movie title");
    expect(actual).toBeInTheDocument();
  });

  it("should show all movie props", () => {
    render(<Detail {...defaultMovie} />);
    const propsToDisplay = ["title", "overview", "posterImage"];
    propsToDisplay.forEach((prop: string) => {
      const key = defaultMovie[prop] as string;
      expect(screen.getByText(key)).toBeInTheDocument();
    });
  });

  it("should throw error if id missing", async () => {
    const context = {
      req: jest.fn() as TestReq,
      res: jest.fn() as TestRes,
      query: {},
      resolvedUrl: "",
    };

    try {
      await getServerSideProps(context);
      expect(true).toBeFalsy();
    } catch (e: any) {
      expect(e.message).toEqual("Missing id");
    }
  });

  it("should fetch movie when id given", async () => {
    const context = {
      req: jest.fn() as TestReq,
      res: jest.fn() as TestRes,
      query: {},
      params: {
        id: "1",
      },
      resolvedUrl: "",
    };

    (movieService.getById as jest.Mock<any>).mockResolvedValue(defaultMovie);

    const props = await getServerSideProps(context);

    expect(props).toEqual({
      props: {
        id: 1,
        title: "Movie title",
        overview: "Overview summary text",
        posterImage: "image",
        genres: ["Romance", "Comedy"],
        homepage: "https://net.com",
        spokenLanguages: ["Italian"],
      },
    });
  });
});
