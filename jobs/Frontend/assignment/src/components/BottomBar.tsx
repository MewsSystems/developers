import { ReactNode } from "react";
import styled from "styled-components";
import { IconButton } from ".";
import ThemeModeIcon from "@material-ui/icons/WbSunnyOutlined";
import { useDarkMode } from "@/hooks";

export interface BottomBarProps {
  leftChildren?: ReactNode;
}

const BottomBarContainer = styled.div`
  position: fixed;
  bottom: 0;
  left: 0;

  display: flex;
  align-items: center;
  justify-content: space-between;

  width: 100%;
  padding: 12px 16px;

  backdrop-filter: blur(10px);
  border-top: 1px solid ${({ theme }) => theme.colors.outline.variant};

  &:before {
    content: " ";
    position: absolute;
    top: 0;
    left: 0;
    z-index: -1;

    width: 100%;
    height: 100%;
    opacity: 0.8;

    background-color: ${({ theme }) => theme.colors.surface.main};
  }
`;

export function BottomBar({ leftChildren }: BottomBarProps) {
  const { toggleDarkMode } = useDarkMode();

  return (
    <BottomBarContainer>
      <div>{leftChildren}</div>
      <IconButton size="large" onClick={toggleDarkMode}>
        <ThemeModeIcon />
      </IconButton>
    </BottomBarContainer>
  );
}
