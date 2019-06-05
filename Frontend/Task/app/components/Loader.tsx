import React from "react";
import { LoaderBackdrop, LoaderCircle, Paragraph, Spacer } from "./styled";

export const Loader = () => (
  <LoaderBackdrop>
    <LoaderCircle />
    <Spacer top>
      <Paragraph textAlign="center">loading rates</Paragraph>
    </Spacer>
  </LoaderBackdrop>
);
