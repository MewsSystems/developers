import { ChangeEventHandler, FC } from "react";
import { SearchInput } from "./styled";

type Props = {
  onChange: ChangeEventHandler<HTMLInputElement>;
  value: string;
};

export const ListSearchInput: FC<Props> = ({ onChange, value }) => {
  return (
    <SearchInput
      type="text"
      id="movie-search-input"
      placeholder="Start typing to view movie list"
      onChange={onChange}
      value={value}
    />
  );
};
