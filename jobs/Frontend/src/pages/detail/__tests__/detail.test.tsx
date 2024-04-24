import { render, screen } from "@testing-library/react";

jest.mock("next/image", () => (props) => <span>{props.src}</span>);

import { Movie } from "@/types";
import Detail, { getServerSideProps } from "../[id]";
import { GetServerSideProps } from "next";
import { IncomingMessage, ServerResponse } from "http";
import { NextApiRequestCookies } from "next/dist/server/api-utils";
import { movieService } from "@/services/movieService";

jest.mock("../../../services/movieService");

type TestReq = IncomingMessage & {
  cookies: NextApiRequestCookies;
} & jest.Mock<any>;

type TestRes = ServerResponse & jest.Mock<any>;

const defaultMovie: Movie & { [key: string]: string | number | undefined } = {
  id: 1,
  title: "Movie title",
  overview: "Overview summary text",
  posterImage: "image",
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

    const defaultMovie: Movie = {
      id: 1,
      title: "test",
      overview: "overview",
      posterImage: "https://w92/1.jpg",
    };

    (movieService.getById as jest.Mock<any>).mockResolvedValue(defaultMovie);

    const props = await getServerSideProps(context);

    expect(props).toEqual({
      props: {
        id: 1,
        title: "test",
        overview: "overview",
        posterImage: "https://w185/1.jpg",
      },
    });
  });
});
