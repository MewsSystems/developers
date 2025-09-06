// components/EmptyState.tsx
import type { ReactNode } from "react";
import { EmptyStateContainer, EmptyStateMessage } from "../styles/styles";

type EmptyStateProps = {
  message: string;
  icon?: ReactNode;
};

const EmptyState = ({ message, icon }: EmptyStateProps) => {
  return (
    <EmptyStateContainer>
      {icon && (
        <div style={{ fontSize: "40px", marginBottom: "10px" }}>{icon}</div>
      )}
      <EmptyStateMessage>{message}</EmptyStateMessage>
    </EmptyStateContainer>
  );
};

export default EmptyState;
