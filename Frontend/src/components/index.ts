import styled from "styled-components";
import { Link } from "react-router-dom";

export const Layout = styled.div`
  display: grid;
  grid-template-columns: 350px 1fr 350px;
  height: 100vh;
`;

export const CenterColumn = styled.div`
  grid-column: 2;
  padding: 20px;
`;

export const Card = styled.div`
  box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
  display: flex;
  padding: 10px;
  color: gray;
  font: 16px/1.5 Helvetica, Verdana, sans-serif;
`

export const Content = styled.div`
  padding: 2px 16px;
`

export const Title = styled.div`
  display: block;
  margin: 10px 0;
  font-weight: bold;
  color: black;
  font-size:20px;
`;

export const SubTitle = styled.div`
  font-size:14px;
  color:grey;
  margin:2px 0;
`;

export const Paragraph = styled.p`
  line-height: 18px;
  margin: 5px;
`;

export const ErrorMessage = styled.div`
  border:1px solid red;
  padding:10px;
  color:red;
  font: 18px Helvetica, Verdana, sans-serif;
`



export const StyledInput = styled.input`
  background-color: transparent;
  border: none;
  border-bottom: 1px solid #9e9e9e;
  border-radius: 0;
  outline: none;
  height: 3rem;
  width: 100%;
  font-size: 16px;
  margin: 0 0 8px 0;
  padding: 0;
  box-shadow: none;
  box-sizing: content-box;
  transition: box-shadow .3s, border .3s;
`;

export const StyledLabel = styled.label`
  width:100%;
  color: #9e9e9e;
  position: absolute;
  top: -15px;
  left: 0;
  font-size: 1rem;
  cursor: text;
  transition: transform .2s ease-out, color .2s ease-out;
  text-align: initial;
  &::after {
    display: block;
    content: "";
    position: absolute;
    top: 100%;
    left: 0;
    opacity: 0;
    transition: .2s opacity ease-out, .2s color ease-out;
  }
`

export const Button = styled.button`
  text-decoration: none;
  color: #fff;
  background-color: #26a69a;
  text-align: center;
  letter-spacing: .5px;
  transition: background-color .2s ease-out;
  cursor: pointer;
  font-size: 14px;
  outline: 0;
  border: none;
  border-radius: 2px;
  display: inline-block;
  height: 36px;
  line-height: 36px;
  padding: 0 16px;
  text-transform: uppercase;
  vertical-align: middle;
  &:disabled {
    background-color:#ccc;
  }
`;

export const StyledLink = styled(Link)`
  text-decoration: none;
  color: #26a69a;
`;
