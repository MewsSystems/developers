import React, { useState } from "react";
import { formatLanguageCodeToString } from "../../../../utils/formatLanguageCodeToString";
import { Table } from "../../../../components/Table/Table";
import { TableDataCell } from "../../../../components/TableDataCell/TableDataCell";
import { MovieDetailsDrawer } from "../MovieDetailsDrawer/MovieDetailsDrawer";
import { ExtendedMovie } from "../../../../types";

type MovieTableProps = {
  data: ExtendedMovie[];
}

type Column = {
  key: keyof ExtendedMovie;
  label: string;
  render: (a: ExtendedMovie) => React.JSX.Element;
}

const columns: Column[] = [
  {
    key: "title",
    label: "Title",
    render: ({ title }) => (
      <TableDataCell key="title" label={title} maxWidth={200} />
    )
  },
  {
    key: "overview",
    label: "Summary",
    render: ({ overview }) => (
      <TableDataCell key="overview" label={overview} maxWidth={500} />
    )
  },
  {
    key: "releaseDate",
    label: "Release date",
    render: ({ releaseDate }) => (
      <TableDataCell key="releaseDate" label={releaseDate} />
    )
  },
  {
    key: "originalLanguage",
    label: "Language",
    render: ({ originalLanguage }) => (
      <TableDataCell 
        key="originalLanguage" 
        label={formatLanguageCodeToString(originalLanguage)}
      />
    )
  }
];

export const MovieTable = ({ data }: MovieTableProps) => {
  const [ selectedMovieId, setSelectedMovieId ] = useState<number>();
  const [ isDrawerOpen, setIsDrawerOpen ] = useState(false);

  return (
    <>
      <Table
        columns={columns}
        data={data}
        onClickRow={movie => {
          setSelectedMovieId(movie.id);
          setIsDrawerOpen(true);
        }}
      />
      <MovieDetailsDrawer
        id={selectedMovieId} 
        isOpen={isDrawerOpen} 
        onClose={() => setIsDrawerOpen(false)}
      />
    </>
  );
};
