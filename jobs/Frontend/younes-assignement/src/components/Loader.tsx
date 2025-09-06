import { Loader as LoaderIcon } from "lucide-react";
import {
  EmptyStateContainer,
  EmptyStateMessage,
  Spinner,
} from "../styles/styles";

const Loader = () => (
  <EmptyStateContainer>
    <Spinner>
      <LoaderIcon size={40} />
    </Spinner>
    <EmptyStateMessage>Loading...</EmptyStateMessage>
  </EmptyStateContainer>
);

export default Loader;
