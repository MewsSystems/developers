import React from "react";
import { Drawer } from "../../../../components/Drawer/Drawer";
import { MovieDetails } from "./components/MovieDetails";

type MovieDetailsDrawerProps = {
  id: number | undefined;
  isOpen: boolean;
  onClose: () => void;
}

export const MovieDetailsDrawer = ({ id, isOpen, onClose }: MovieDetailsDrawerProps) => {
  return (
    <Drawer anchor="right" isOpen={isOpen} onClose={onClose}>
      {id && <MovieDetails id={id} />}
    </Drawer>
  );
};
