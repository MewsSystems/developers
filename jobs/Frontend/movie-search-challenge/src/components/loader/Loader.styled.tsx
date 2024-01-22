import styled from "styled-components"

export const StyledLoader = styled.span`
  margin: auto;
  width: 100px;
  height: 30px;
  overflow: hidden;
  position: relative;
  background: rgba(0, 0, 0, 0.3);
  border-radius: 5px;
  box-shadow:
    0px 35px 0 -5px #aaa,
    0 -5px 0 0px #ddd,
    0 -25px 0 -5px #fff,
    -25px -30px 0 0px #ddd,
    -25px 30px 0 0px #ddd,
    25px -30px 0 0px #ddd,
    25px 30px 0 0px #ddd,
    20px 10px 0 5px #ddd,
    20px -10px 0 5px #ddd,
    -20px -10px 0 5px #ddd,
    -20px 10px 0 5px #ddd;

  &:after,
  &:before {
    content: "";
    border-radius: 100%;
    width: 35px;
    height: 35px;
    display: block;
    position: absolute;
    border: 4px dashed #fff;
    bottom: -4px;
    transform: rotate(0deg);
    box-sizing: border-box;
    animation: tape 4s linear infinite;
  }
  &:before {
    right: 0;
    box-shadow:
      0 0 0 4px #fff,
      0 0 0 34px #000;
  }
  &:after {
    left: 0;
    box-shadow:
      0 0 0 4px #fff,
      0 0 0 65px #000;
  }

  @keyframes tape {
    0% {
      transform: rotate(0deg) scale(0.4);
    }
    100% {
      transform: rotate(-360deg) scale(0.4);
    }
  }
`
