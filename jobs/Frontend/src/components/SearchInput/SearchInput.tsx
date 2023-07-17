import {
  ChangeEventHandler,
  ComponentProps,
  useCallback,
  useMemo,
} from "react";
import { styled } from "styled-components";
import { debounce } from "../../utils";
import { Icon } from "../Icon";
import { Card } from "../Card";

const Container = styled.div`
  position: relative;
  width: 100%;
`;

const Input = styled(Card)`
  height: 48px;
  width: 100%;
  border: none;

  font-size: 1rem;

  padding: 0 4px;
  padding-left: 48px;
`;

Input.defaultProps = {
  as: "input",
};

const IconWrapper = styled.div`
  position: absolute;
  left: 16px;
  top: 50%;
  transform: translateY(-50%);
`;

type BaseSearchInputProps = {
  defaultQuery?: string;
  onChange: (query: string) => void;
};

type SearchInputProps = Omit<
  ComponentProps<"input">,
  keyof BaseSearchInputProps
> &
  BaseSearchInputProps;

export const DEFAULT_PLACEHOLDER = "Search for a movie title...";

export const SearchInput = ({
  defaultQuery = "",
  onChange,
  placeholder = DEFAULT_PLACEHOLDER,
  ...rest
}: SearchInputProps) => {
  const debouncedChange = useMemo(() => debounce(onChange, 500), [onChange]);

  const handleChange: ChangeEventHandler<HTMLInputElement> = useCallback(
    (e) => {
      debouncedChange(e.currentTarget.value);
    },
    [debouncedChange]
  );

  return (
    <Container>
      <Input
        onChange={handleChange}
        defaultValue={defaultQuery}
        name="search"
        type="search"
        placeholder={placeholder}
        {...rest}
      />
      <IconWrapper>
        <Icon.Search />
      </IconWrapper>
    </Container>
  );
};
