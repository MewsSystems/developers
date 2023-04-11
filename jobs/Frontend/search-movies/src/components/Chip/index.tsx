import styled from "styled-components";
import { colors } from "../../utils/theme";
import { shadowSm } from "../General";

const ChipContainer = styled.div`
  padding: 5px 20px;
  margin: 10px 20px 10px 0px;
  height: 20px;
  border-radius: 30px;
  color: ${colors.primaryText};
  box-shadow: ${shadowSm};
  justify-content: center;
  white-space: nowrap;
  display: flex;
  font-size: 14px;
`;

const ChipLabel = styled.div`
  white-space: nowrap;
`;

interface ChipProps {
  label: string | number;
  heading?: string;
}

/**
 * Chip with label and heading
 * @param props {label, heading} label and optional heading for a chip
 * @returns renders a chip with label and optional heading
 */
const Chip = (props: ChipProps) => {
  const { label, heading } = props;
  return (
    <ChipContainer>
      {heading ? (
        <ChipLabel>
          {heading} {label}
        </ChipLabel>
      ) : (
        <ChipLabel> {label}</ChipLabel>
      )}
    </ChipContainer>
  );
};

export default Chip;
