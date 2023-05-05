import { LoadingOutlined, SearchOutlined } from "@ant-design/icons";
import { StyledInput } from "./SearchInput.styled";

type SearchInputProps = {
  loading?: boolean;
  placeholder?: string;
};

const SearchInput = ({
  loading,
  placeholder = "Type here to start searching",
}: SearchInputProps) => (
  <StyledInput
    placeholder={placeholder}
    suffix={loading ? <LoadingOutlined /> : <SearchOutlined />}
  />
);

export { SearchInput };
