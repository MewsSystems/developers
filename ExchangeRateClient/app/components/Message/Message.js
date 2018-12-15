import React from "react";
import styled from "styled-components";

const Container = styled.span`
  padding: 30px 0;
`;

const Message = ({ content }) => <Container>{content}</Container>;

export default Message;
