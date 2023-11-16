import styled from "styled-components";

const ErrorMessage = styled.h3`
  font-family: Exo;
  font-size: 24px;
  color: whitesmoke;
`;

const ErrorBox = styled.div`
  background-color: darkred;
  padding: 8px;
  border-radius: 8px;
`;

interface IErrorNotification {
  message?: string;
}

export function ErrorNotification(props:IErrorNotification) {
  return (
    <ErrorBox>
      <ErrorMessage>{props.message != undefined ? props.message : "Something went wrong, please try again later"}</ErrorMessage>
    </ErrorBox>
  );
}
