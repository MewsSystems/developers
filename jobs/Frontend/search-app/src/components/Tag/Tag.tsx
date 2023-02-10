import { TagStyled } from "./Tag.styled";

type Props = {
  value: string;
};

export const Tag = ({ value }: Props) => {
  return <TagStyled data-test="tag">{value}</TagStyled>;
};
