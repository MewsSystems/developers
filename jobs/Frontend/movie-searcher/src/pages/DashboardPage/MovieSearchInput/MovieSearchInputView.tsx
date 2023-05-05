import { LoadingOutlined, SearchOutlined } from "@ant-design/icons";
import { Col, Row } from "antd";
import { ChangeEventHandler } from "react";
import { StyledInput } from "./SearchInput.styled";

type MovieSearchInputViewProps = {
  searchInput?: string;
  isLoading?: boolean;
  searchChangeHandler: ChangeEventHandler<HTMLInputElement>;
};

const MovieSearchInputView = ({
  searchInput,
  searchChangeHandler,
  isLoading,
}: MovieSearchInputViewProps) => (
  <Row>
    <Col span={12} offset={6}>
      <StyledInput
        placeholder="Enter the title of the movie"
        suffix={isLoading ? <LoadingOutlined /> : <SearchOutlined />}
        value={searchInput}
        onChange={searchChangeHandler}
      />
    </Col>
  </Row>
);

export { MovieSearchInputView };
