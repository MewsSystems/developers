import { ReactNode } from "react";
import { RatingStyled } from "./Rating.styled";

type Props = {
  label: string;
  isHighlighted: boolean;
  iconName: string;
  children: ReactNode;
};

export const Rating = ({ label, isHighlighted, iconName, children }: Props) => {
  return (
    <RatingStyled data-test="rating">
      <div>{label}</div>
      <div className={isHighlighted ? "highlighted" : ""}>
        <i className={iconName}></i>
        <span>{children}</span>
      </div>
    </RatingStyled>
  );
};
