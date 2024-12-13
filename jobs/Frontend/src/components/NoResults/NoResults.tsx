import {
  NoResultsContainer,
  NoResultsIcon,
  NoResultsMessage,
} from "@/components/NoResults/NoResultsStyle";

function NoResults({ message = "Try adjusting your search..." }) {
  return (
    <NoResultsContainer>
      <NoResultsIcon>🔍</NoResultsIcon>
      <NoResultsMessage>{message}</NoResultsMessage>
    </NoResultsContainer>
  );
}

export default NoResults;
