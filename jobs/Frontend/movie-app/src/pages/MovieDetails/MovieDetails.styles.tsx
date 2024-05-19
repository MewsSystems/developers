import styled from "styled-components";

export const MovieDetailContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  padding: 0 2rem 2rem 2rem;
`;
export const Header = styled.h1`
  margin-bottom: 0.5rem;
`;
export const HeaderInfo = styled.div`
  display: flex;
  gap: 0.5rem;
  color: #808080;
`;
export const MovieImageWrapper = styled.div`
  display: block;
  width: 640px;
  height: 360px;
  border-radius: 1rem;
  background: #efefef;
  animation: loading 1s linear infinite alternate;
  @keyframes loading {
    0% {
      background-color: hsl(200, 20%, 80%);
    }
    100% {
      background-color: hsl(200, 20%, 95%);
    }
  }
  @media (max-width: 1064px) {
    width: 100%;
    height: 100%;
  }
`;
export const MovieImage = styled.img`
  width: 640px;
  height: 360px;
  border-radius: 1rem;
  @media (max-width: 1064px) {
    width: 100%;
    height: 100%;
  }
`;
export const MovieDataContainer = styled.div`
  width: 640px;
  @media (max-width: 1064px) {
    width: 100%;
    height: 100%;
  }
`;
export const GenresContainer = styled.div`
  display: flex;
  margin: 1rem 0;
  gap: 1rem;
`;
export const Pill = styled.div`
  background: #efefef;
  padding: 0.25rem 1rem;
  border-radius: 1rem;
  display: flex;
  align-items: center;
`;
