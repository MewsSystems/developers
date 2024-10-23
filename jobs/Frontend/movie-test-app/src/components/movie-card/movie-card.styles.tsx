import styled from 'styled-components';

export const MovieImageContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  height: 100%;
`;

export const MovieCardContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  position: relative;
  height: 100%;

  img {
    width: 100%;
    height: 90%;
    object-fit: cover;
    margin-bottom: 5px;
  }

  button {
    width: 80%;
    opacity: 0.7;
    position: absolute;
    top: 255px;
    display: none;
  }

  &:hover {
    img {
      opacity: 0.8;
    }

    button {
      opacity: 0.85;
      display: flex;
    }
  }
`;

export const Footer = styled.div`
  width: 100%;
  height: 10%;
  display: flex;
  flex-direction: row;
  justify-content: center;
  text-align: center;
  font-size: 18px;
`;

export const Name = styled.span`
  width: 90%;
  margin-bottom: 15px;
`;
