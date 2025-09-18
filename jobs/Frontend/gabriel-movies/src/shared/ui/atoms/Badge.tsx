import styled from "styled-components";

type Variant = "success" | "warning" | "danger";

export const Badge = styled.span<{ $variant: Variant }>`
  padding: 4px 10px;
  border-radius: 20px;
  font-weight: 600;
  border: 1px solid currentColor;
  color: ${({ $variant, theme }) => theme.colors[$variant]};
`;
