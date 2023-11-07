import styled from "styled-components";
import EmptyStar from "@/assets/icons/empty-star.svg";
import FilledStar from "@/assets/icons/filled-star.svg";
import HalfFilledStar from "@/assets/icons/half-filled-star.svg";

export interface RatingProps {
  value: number;
}

const StyledWrapper = styled.div`
  display: flex;
  gap: 3px;
`;

const StyledStar = styled.div<{ $img: string }>`
  width: 15px;
  height: 15px;

  background-image: url(${props => props.$img});
  background-size: contain;
`;

export const getStarIcon = (value: number, index: number) => {
  if (value >= index) return FilledStar;

  if (value >= index - 0.5) return HalfFilledStar;

  return EmptyStar;
};

export function Rating({ value }: RatingProps) {
  const roundedValue = Math.round(value * 2) / 2;

  return (
    <StyledWrapper>
      {[1, 2, 3, 4, 5].map(index => (
        <StyledStar key={index} data-testid="star-icon" $img={getStarIcon(roundedValue, index)} />
      ))}
    </StyledWrapper>
  );
}
