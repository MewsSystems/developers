import styled, { css } from "styled-components";
import Img from "react-image";

export const Header = styled.h2`
  line-height: 14px;
  color: gray;
  font: bolder 30px/1.5 Helvetica, Verdana, sans-serif;
  margin: 5px;
`;

export const Paragraph = styled.p`
  line-height: 14px;
  color: gray;
  font: 16px/1.5 Helvetica, Verdana, sans-serif;
  margin: 5px;
`;

export const Button = styled.button`
  display: inline-block;
  border-radius: 3px;
  width: 200px;
  margin: 10px 0 0 0;
  height: 52px;
  background: lightblue;
  color: white;
  border: 1px solid white;
  font: bolder 16px/1.5 Helvetica, Verdana, sans-serif;
  &:hover {
    background: #4897c9;
    cursor: pointer;
  }
  &:disabled {
    background: #dddddd;
  }
`;

export const BackButton = styled(Button)`
  position: absolute;
  left: 1%;
`;

export const InputLabel = styled.label`
  font-family: "Roboto", sans-serif;
  font-size: 1.2rem;
  margin-left: 2rem;
  margin-top: 0.7rem;
  display: block;
  transition: all 0.3s;
  transform: translateY(0rem);
`;

export const Input = styled.input`
  font-family: "Roboto", sans-serif;
  color: #333;
  font-size: 1rem;
  margin: 10px;
  padding: 1rem;
  border-radius: 0.2rem;
  border: none;
  width: 90%;
  display: block;
  border-bottom: 0.3rem solid transparent;
  transition: all 0.3s;
  border: 0.5px solid lightblue;
`;

export const Image = styled.img`
  border-radius: 2%;
  width: 200px;
  height: 200px;
  float: left;
  margin: 5px;
`;

export const Checkbox = styled.input`
  padding: 10px;
  margin: 10px;
`;

export const Div = css`
  font-family: "Roboto", sans-serif;
  color: #333;
  font-size: 1rem;
  margin: 0 auto;
  padding: 1rem;
  border-radius: 0.2rem;
  background-color: rgb(255, 255, 255);
  border: none;
  width: 90%;
  display: block;
  transition: all 0.3s;
`;

export const List = styled.ul`
  list-style-position: inside;
  border-radius: 5px;
  margin: 10px 0px 10px 10px;
  color: black;
  list-style-type: none;
  padding: 0;
`;

export const ListItem = styled.li`
  overflow: auto;
  list-style-position: inside;
  margin: 0 0 20px 0;
  color: black;
  border: 1px solid #e1e4e8 !important;
  border-radius: 3px;
  z-index: 100;
  &:hover {
    border: 1px solid lightblue !important;
    background-color: #faffff;
    cursor: pointer;
  }
`;

export const ThumbnailImage = styled(Img)`
  display: block;
  max-width: 133px;
  max-height: 200px;
  width: auto;
  height: "auto";
  border-radius: 5px;
  vertical-align: middle;
`;

export const PosterImage = styled(Img)`
  max-width: 266px;
  max-height: 400px;
`;

export const VerticalDiv = styled.div`
  display: flex;
  margin: 10px;
`;

export const MovieDetailDiv = styled(VerticalDiv)`
  flex-direction: column;
`;
