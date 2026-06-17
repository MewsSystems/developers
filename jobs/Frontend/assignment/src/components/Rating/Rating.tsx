import styled from "styled-components";
import StarFilledIcon from "@material-ui/icons/Star";
import StarHalfFilledIcon from "@material-ui/icons/StarHalf";
import StarEmptyIcon from "@material-ui/icons/StarOutline";

export interface RatingProps {
  value: number;
}

const StyledWrapper = styled.div`
  display: flex;
  gap: 1px;
`;

const StyledStar = styled.div<{ $secondary: boolean }>`
  width: 20px;
  height: 20px;

  color: ${({ theme, $secondary }) => ($secondary ? theme.colors.outline.variant : "#ffb400")};
`;

export function Rating({ value }: RatingProps) {
  const roundedValue = Math.round(value * 2) / 2;

  return (
    <StyledWrapper>
      {[1, 2, 3, 4, 5].map(index => (
        <StyledStar key={index} data-testid="star-icon" $secondary={roundedValue < index}>
          {(roundedValue >= index && (
            <StarFilledIcon fontSize="small" data-testid="filled-star-icon" />
          )) ||
            (roundedValue >= index - 0.5 && (
              <StarHalfFilledIcon fontSize="small" data-testid="half-star-icon" />
            )) ||
            (roundedValue < index && (
              <StarEmptyIcon fontSize="small" data-testid="empty-star-icon" />
            ))}
        </StyledStar>
      ))}
    </StyledWrapper>
  );
}
