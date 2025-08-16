import React from "react";
import { render, screen } from "@testing-library/react";
import { userEvent } from "@testing-library/user-event";
import { Table } from "./Table";

type Data = {
  name: string;
  alias: string;
}

const data: Data[] = [
  {
    name: "Peter Parker",
    alias: "Spiderman"
  },
  {
    name: "Bruce Wayne",
    alias: "Batman"
  }
];

type Columns = {
  key: keyof Data;
  label: string;
  render: (a: Data) => React.JSX.Element;
}

const columns: Columns[] = [
  {
    key: "name",
    label: "Name",
    render: ({ name }) => <td key={name}>{name}</td>
  },
  {
    key: "alias",
    label: "Alias",
    render: ({ alias }) => <td key={alias}>{alias}</td>
  }
];

const mockFn = jest.fn();

describe("<Table />", () => {
  it("should display the correct column headers", () => {
    render(
      <Table columns={columns} data={data} onClickRow={mockFn} />
    );

    const columnHeaders = screen.getAllByRole("columnheader");

    columnHeaders.forEach((columnHeader, i) => {
      expect(columnHeader.textContent).toMatch(columns[i].label);
    });
  });

  it("should display the correct cell data in the correct columns", () => {
    render(
      <Table columns={columns} data={data} onClickRow={mockFn} />
    );

    const firstRow = screen.getByRole("row", {  name: /peter parker spiderman/i });
    expect(firstRow).toBeInTheDocument();

    const secondRow = screen.getByRole("row", {  name: /bruce wayne batman/i });
    expect(secondRow).toBeInTheDocument();
  });

  it("should call the onClickRow function with the correct argument", async () => {
    render(
      <Table columns={columns} data={data} onClickRow={mockFn} />
    );

    await userEvent.click(screen.getByRole("row", {  name: /bruce wayne/i }));
    expect(mockFn).toHaveBeenCalledWith(data[1]);
  });
});
