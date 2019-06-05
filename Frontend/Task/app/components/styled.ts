import styled, { css, keyframes } from "styled-components";
import { padding, margin, position, size, rgba } from "polished";
import { theme } from "../config/theme";
import * as React from "react";

export interface FlexParentProps {
  alignItems?: "center" | "flex-start" | "flex-end";
  justifyContent?:
    | "center"
    | "flex-start"
    | "flex-end"
    | "space-between"
    | "space-around";
  flexWrap?: boolean;
  flexDirection?: "row" | "column";
  fullWidth?: boolean;
  isInline?: boolean;
}

export interface FlexChildProps {
  flexGrow?: number;
  flexShrink?: number;
  width?: string;
}

export interface TypographyProps {
  textAlign?: "left" | "center" | "right";
}

export interface ListItemProps {
  withBorder?: boolean;
}

export const List = styled("ul")({
  ...padding(0),
  ...margin(0, 0, theme.baseSpacing * 0.5, 0),
  listStyle: "none"
});

export const ListItem = styled("li")<ListItemProps>`
  padding: 0;
  ${({ withBorder }) =>
    withBorder ? `border-bottom: 1px solid ${theme.borderColor};` : ""}
  &:last-of-type {
    border-bottom: 0;
  }
`;

export const Paragraph = styled("p")<TypographyProps>`
  font-family: sans-serif;
  font-size: 0.8rem;
  margin: 0;
  ${({ textAlign }) => (textAlign ? `text-align: ${textAlign};` : "")};
`;

interface SpacerProps {
  top?: boolean;
  bottom?: boolean;
}

export const Spacer = styled("div")<SpacerProps>`
  ${({ top }) => (top ? `padding-top: ${theme.baseSpacing}px;` : "")}
  ${({ bottom }) => (bottom ? `padding-top: ${theme.baseSpacing}px;` : "")}
`;

export const CellTitle = styled("h3")<TypographyProps>`
  font-size: 1rem;
  font-weight: bold;
  font-family: sans-serif;
  ${({ textAlign }) => (textAlign ? `text-align: ${textAlign};` : "")}
`;

export const Title = styled("h1")`
  font-family: sans-serif;
  font-size: 2rem;
  text-align: center;
`;

export const Container = styled("main")({
  maxWidth: theme.containerWidth,
  ...margin(null, "auto"),
  ...padding(null, theme.baseSpacing * 2),
  position: "relative",
  minHeight: theme.minWidth // just for now (Loader)
});

export const FlexParent = styled("div")<FlexParentProps>`
    display: ${({ isInline }) => (isInline ? "inline-flex" : "flex")};
    ${({ fullWidth }) => (fullWidth ? `width: 100%;` : "")}
    ${({ flexDirection }) =>
      flexDirection ? `flex-direction: ${flexDirection};` : ""}
    ${({ alignItems }) => (alignItems ? `align-items: ${alignItems};` : "")}
    ${({ justifyContent }) =>
      justifyContent ? `justify-content: ${justifyContent};` : ""}
    ${({ flexWrap }) => (flexWrap ? "flex-wrap: wrap;" : "")}
`;

// we set the width of element by flex-basis & max-width + inline-block (IE fix)
export const FlexChild = styled("div")<FlexChildProps>`
  display: inline-block;
  flex-grow: ${({ flexGrow }) => flexGrow || 0};
  flex-shrink: ${({ flexShrink }) => flexShrink || 0};
  flex-basis: ${({ width }) => width || "auto"};
  max-width: ${({ width }) => (width ? width : "100%")};
`;

export const CellRow = styled(FlexParent)`
  justify-content: space-between;
`;

export const CellStyled = styled(FlexChild)<TypographyProps>`
  max-width: ${theme.minWidth / 3}px;
  flex-basis: ${theme.minWidth / 3}px;
  ${padding(theme.baseSpacing * 0.5, theme.baseSpacing)};
  flex-shrink: 1;
`;

const pseudoStyles = css`
  ${position("absolute", null, 0, 0, 0)};
  display: inline-block;
  content: "";
  height: 1px;
  width: 100%;
`;

export const InputWrapper = styled("div")`
  position: relative;
  &:before {
    ${pseudoStyles};
    background-color: ${theme.borderColor};
  }
  &:after {
    ${pseudoStyles};
    background-color: ${theme.emphColor};
    will-change: transform;
    transition-duration: ${theme.transitionDuration}ms;
    transition-timing-function: ease;
    transform: scaleX(0);
  }
  &:hover,
  &:focus {
    &:after {
      transform: scaleX(1);
    }
  }
`;

export interface InputStyledInterface {
  onChange: (e: React.FormEvent<HTMLInputElement>) => void;
}

export const InputStyled = styled.input.attrs({ type: "text" })<
  InputStyledInterface & React.HTMLProps<HTMLInputElement>
>`
  border: 0;
  outline: 0;
  height: ${theme.baseSpacing * 2}px;
  line-height: ${theme.baseSpacing * 2}px;
  font-size: 1rem;
  font-family: sans-serif;
  ${padding(null, theme.baseSpacing * 0.5)};
  width: 100%;
  text-align: center;
  &:focus,
  &:hover {
    border: 0;
    outline: 0;
  }
`;

const spin = keyframes`
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
`;

export const ExchangeWrapper = styled("div")`
  min-height: ${theme.minWidth}px;
  position: relative;
`;

export const LoaderBackdrop = styled("div")`
  ${position("absolute", 0, 0, 0, 0)};
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  background-color: ${rgba(theme.borderColor, 0.5)};
  z-index: 1000;
  pointer-events: none;
`;

export const LoaderCircle = styled.div`
  position: relative;
  display: inline-block;
  ${size(theme.baseSpacing * 3)};

  &::after {
    box-sizing: border-box;
    width: 100%;
    height: 100%;
    display: block;
    border-top-color: ${rgba(theme.emphColor, 0.3)};
    border-right-color: ${rgba(theme.emphColor, 0.3)};
    border-bottom-color: ${rgba(theme.emphColor, 0.3)};
    border-left-color: ${rgba(theme.emphColor, 0.9)};
    border-style: solid;
    border-width: ${theme.baseSpacing * 0.25}px;
    border-radius: 50%;
    content: "";
    animation: ${spin} 1s infinite linear;
  }
`;
