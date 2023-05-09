import { styled } from "styled-components";

export const ListWrapper = styled.div`
  display: flex;
  flex-direction: column;
`;

export const MovieListWrapper = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 1rem;
`;

export const MovieListItem = styled.div`
  padding: 1rem;
  background-color: #282828;
  border-radius: 4px;
  cursor: pointer;
  transition: 0.2s;

  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
  }

  img {
    max-width: 100%;
    border-radius: 4px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    margin-bottom: 1rem;
  }
`;

export const RatingWrapper = styled.div`
  display: flex;
`;

export const ShowMoreButton = styled.div`
  display: flex;

  button {
    width: 100%;
    height: 40px;
    padding: 0.5rem;
    font-size: 1rem;
    border-radius: 8px;
    background-color: #f1c40f;
    color: #fff;
    outline: none;
    border: none;
    margin-top: 50px;
    cursor: pointer;
  }
`;

