import styled from "styled-components";

export const HeaderBar = styled.div`
  background: #333;
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
  height: 6.5rem;
`;

export const HeaderInput = styled.input`
  font-size: 1.6rem;
  padding: 1rem;
  width: 60%;
  margin: 0.5rem;
  border: 3px solid #999;
  background: #555;
  color: white;
  border-radius: 5px;
  font-weight: bold;
  outline: none;

  &:focus {
    border-color: #aaa;
    background: #666;
  }
`;

export const BackButton = styled.button`
  position: absolute;
  top: 1rem;
  left: 1rem;
  box-sizing: border-box;
  padding: 1rem 2rem;
  height: 4.5rem;
  font-size: 1.6rem;
  border: 3px solid #999;
  background: #555;
  border-radius: 5px;
  color: white;

  &:hover {
    border-color: #aaa;
    background: #666;
    cursor: pointer;
  }
`;

export const TitleWrapper = styled.div`
  color: white;
  display: flex;
  flex-direction: column;
  text-align: center;
  padding: 1rem;
`;

export const Title = styled.h1`
  font-size: 1.8rem;
  font-weight: bold;
`;

export const Subtitle = styled.h2`
  font-size: 1.4rem;
  font-weight: normal;
`;
