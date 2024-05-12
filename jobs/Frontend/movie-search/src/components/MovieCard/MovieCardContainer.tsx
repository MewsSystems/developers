"use client";
import React from "react";
import { GridContainer } from "./MovieCardStyledComponents";

interface MovieCardContainerProps {
  children: React.ReactNode;
}

export default function MovieCardContainer({
  children,
}: MovieCardContainerProps) {
  return <GridContainer>{children}</GridContainer>;
}
