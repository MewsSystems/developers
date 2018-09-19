// @flow
import { withTheme } from "styled-components";
import * as React from "react";

import { RotateScale } from "styled-loaders-react";

const Spinner = ({ theme }) => <RotateScale size="100px" color={theme.colors.color5} />;

export default withTheme(Spinner);
