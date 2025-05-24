import styled from "styled-components";


const Container = styled.section`
  min-height: 40vh;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  padding: 2rem;
  gap: 1rem;
  text-align: center;
  border-radius: 12px;
  box-shadow: 0 4px 14px rgba(0, 0, 0, 0.08);
`;

const Icon = styled.div`
  font-size: 4rem;
`;

const Code = styled.span`
  font-size: 1.2rem;
  font-weight: 600;
  color: #57606a;
`;

const Title = styled.h1`
  font-size: 1.8rem;
  font-weight: bold;
  color: #24292f;
  margin: 0;
`;

const Description = styled.p`
  font-size: 1rem;
  color: #57606a;
  max-width: 400px;
`;

interface ErrorPageProps {
  code: string;
  message: string;
}

export const ErrorComponent = ({ code, message }: ErrorPageProps) => {

  return (
    <Container role="alert" aria-live="assertive">
      <Icon>🚨</Icon>
      <Code>Error {code}</Code>
      <Title>Oops! Something went wrong</Title>
      <Description>{message}</Description>
    </Container>
  );
};
