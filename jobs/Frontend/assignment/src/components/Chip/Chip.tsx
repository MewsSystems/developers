import styled from "styled-components";

export interface ChipProps {
  label: string;
  imagePath?: string;
}

const StyledChipWrapper = styled.div`
  display: flex;
  align-items: center;
  gap: 8px;

  padding: 3px 10px;

  border-radius: 8px;
  border: 1px solid ${({ theme }) => theme.colors.outline.variant};
`;

const StyledChipLabel = styled.span`
  color: ${({ theme }) => theme.colors.surface.onVariant};
  font-size: ${({ theme }) => theme.fonts.labelLarge.fontSize};
  font-weight: ${({ theme }) => theme.fonts.labelLarge.fontWeight};
`;

const StyledChipImage = styled.span<{ $img: string }>`
  width: 18px;
  height: 18px;
  border-radius: 100px;

  background-image: url(${({ $img }) => $img});
  background-size: cover;
`;

export function Chip({ label, imagePath }: ChipProps) {
  return (
    <StyledChipWrapper>
      {imagePath && <StyledChipImage data-testid="chip-image" $img={imagePath} />}
      <StyledChipLabel>{label}</StyledChipLabel>
    </StyledChipWrapper>
  );
}
