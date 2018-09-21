// @flow
import { withTheme } from "styled-components";
import * as React from "react";
import { RotateScale } from "styled-loaders-react";
import type { Theme } from "../theme/types";

const Spinner = ({ theme }: { theme: Theme }) => (
  <RotateScale size="100px" color={theme.colors.color5} />
);

export default withTheme(Spinner);
