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
  cursor: pointer;
  border: 1px solid ${(props) => props.theme.colors.primary};
  border-radius: 1rem;
  transition: all 0.3s ease-in-out;
  box-shadow: 3px 3px 10px 0px ${(props) => props.theme.colors.secondary};

  img {
    width: 100%;
    height: 100%;
    border-top-left-radius: 1rem;
    border-top-right-radius: 1rem;
  }

  &:hover {
    img {
      opacity: 0.8;
    }
  }
`;

export const Footer = styled.div`
  width: 100%;
  height: 4rem;
  display: flex;
  flex-direction: row;
  justify-content: center;
  text-align: center;
  font-size: 1rem;
`;

export const Name = styled.span`
  width: 90%;
  margin: 0.5rem;
  color: ${(props) => props.theme.colors.primary};
`;
