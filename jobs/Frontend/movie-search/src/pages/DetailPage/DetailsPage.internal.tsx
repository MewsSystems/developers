import styled from "styled-components";

export const MovieDetailsWrapper = styled.div`
  width: 100%;
  height: auto;
  display: flex;
  justify-content: flex-start;
  gap: 2rem;

  @media screen and (max-width: 1024px) {
    flex-wrap: wrap;
    justify-content: center;
  }
`;

export const ImageSection = styled.section`
  display: flex;
  align-items: center;
  justify-content: flex-start;
`;

export const MovieDetailsSection = styled.section`
  width: auto;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  gap: 2.4rem;
`;

export const MovieDetailsTagline = styled.p`
  font-size: 1.2rem;
  color: #333;
  font-style: italic;
`;

export const MovieDetailsRow = styled.div`
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
`;

export const MovieDetailsTitle = styled.p`
  font-size: 1.2rem;
  font-weight: 800;
  color: #333;
`;

export const MovieDetailsVote = styled.div`
  display: flex;
  align-items: center;
  justify-content: flex-start;
  gap: 0.8rem;
  border-radius: 100px;
  padding: 1.2rem;
  font-weight: 800;
  background-color: #eee;
`;

export const MovieDetailsList = styled.ul`
  width: 100%;
  padding: 0;
  list-style: none;
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  align-items: center;
  flex-wrap: wrap;
  gap: 0.4rem;
`;

interface MovieDetailsBadgeProps {
  $isInverted?: boolean;
}

export const MovieDetailsBadge = styled.li<MovieDetailsBadgeProps>`
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.2rem;
  padding: 0.2rem;
  border: 1px solid #333;

  background-color: ${(props) => (props.$isInverted ? "#333" : "#fff")};

  span {
    color: ${(props) => (props.$isInverted ? "#fff" : "#333")};
    font-size: 0.8rem;
  }
`;

export const MovieOverview = styled.p`
  color: #333;
  font-size: 0.8rem;
`;
