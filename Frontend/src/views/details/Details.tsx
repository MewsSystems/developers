import React from "react";
import { RouteComponentProps } from "react-router-dom";
import { Card, Image } from "semantic-ui-react";
import styled from "styled-components";

import { getImageUrl, getMovieInfo, MovieInfo } from "../../util/theMovieDb";

const StyledCard = styled(Card)`
  width: 75% !important;
  max-width: 500px !important;
  margin: auto !important;
  margin-top: 3em !important;
`;

const StyledCardDescription = styled(Card.Description)`
  clear: none !important;
`;

const StyledHeader = styled.h1`
  width: fit-content;
  margin: auto;
  line-height: 50vh;
  color: white;
`;

export type DetailsProps = RouteComponentProps<{ id: string }>;

export interface DetailsState {
  data?: MovieInfo;
}

class Details extends React.Component<DetailsProps, DetailsState> {
  state: DetailsState = { data: undefined };

  async componentDidMount() {
    this.setState({
      data: await getMovieInfo(Number(this.props.match.params.id))
    });
  }

  render() {
    const { data } = this.state;
    if (data?.title) {
      return (
        <StyledCard>
          <Card.Content>
            <Image
              src={data.poster_path ? getImageUrl(data.poster_path) : ""}
              size="small"
              floated="left"
            />
            <Card.Header>
              {data.title} ({new Date(data.release_date).getFullYear()})
            </Card.Header>
            <Card.Meta>
              {data.runtime} min | {data.genres.map(g => g.name).join(", ")} |{" "}
              {data.status}
            </Card.Meta>
            <StyledCardDescription>{data.overview}</StyledCardDescription>
          </Card.Content>
        </StyledCard>
      );
    } else if (data) {
      console.error(data);
      return <StyledHeader>{(data as any).status_message}</StyledHeader>;
    } else {
      return <StyledHeader>Loading</StyledHeader>;
    }
  }
}

export default Details;
