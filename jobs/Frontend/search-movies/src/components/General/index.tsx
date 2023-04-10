import styled from "styled-components";
import { colors } from "../../utils/theme";

// Shadow styles
export const shadow = `rgba(255, 255, 255, 0.35) 0px 5px 15px;`;
export const shadowSm = `rgba(0, 0, 0, 0.02) 0px 1px 3px 0px, rgba(255, 255, 255, 0.15) 0px 0px 0px 1px;`;
export const shadowInner = `inset 0 0 5px;
`;

/**
 * Button with primary theme styles
 */
export const StyledButton = styled.button`
  border-radius: 12px;
  background-color: ${colors.primary};
  color: ${colors.primaryText};
  font-size: 16px;
  padding: 18px 32px;
  text-transform: uppercase;
  font-weight: 700;
  letter-spacing: 0.08rem;
  border: none;
  &:hover {
    box-shadow: ${shadowSm};
    border: none;
    background-color: ${colors.primaryDark};
  }
`;

/**
 * Button with secondary theme styles
 */
export const StyledSecondaryButton = styled.button`
  border-radius: 12px;
  background-color: ${colors.secondary};
  color: ${colors.secondaryText};
  font-size: 16px;
  padding: 18px 32px;
  text-transform: uppercase;
  font-weight: 700;
  letter-spacing: 0.08rem;
`;

/**
 * Input with styles
 */
export const StyledInput = styled.input`
  border-radius: 26px;
  border: 1px solid ${colors.primary};
  font-size: 16px;
  padding: 18px 20px;
  font-weight: medium;
  letter-spacing: 0.07rem;
  color: ${colors.black};
  &:hover {
    box-shadow: ${shadowSm};
  }
  &::placeholder {
    opacity: 0.7;
  }
`;

/**
 * Paragraph with bold text
 */
export const BP = styled.p`
  font-weight: bold;
`;
