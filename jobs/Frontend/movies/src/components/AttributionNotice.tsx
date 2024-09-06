import styled from 'styled-components';

function AttributionNotice() {
  return (
    <Small>
      This product uses the TMDB API but is not endorsed or certified by TMDB.
    </Small>
  );
}

const Small = styled.small`
  text-align: center;
  color: var(--color-light-text);
  font-size: 0.7rem;
`;

export default AttributionNotice;
